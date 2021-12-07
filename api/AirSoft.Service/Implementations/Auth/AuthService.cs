﻿
using AirSoft.Data.Entity;
using AirSoft.Service.Common;
using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Auth;
using AirSoft.Service.Contracts.Auth.SignIn;
using AirSoft.Service.Contracts.Auth.SignUp;
using AirSoft.Service.Contracts.Jwt;
using AirSoft.Service.Contracts.Jwt.Model;
using AirSoft.Service.Contracts.Member;
using AirSoft.Service.Contracts.Member.Create;
using AirSoft.Service.Contracts.Models;
using AirSoft.Service.Exceptions;
using Microsoft.Extensions.Logging;

namespace AirSoft.Service.Implementations.Auth;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly IJwtService _jwtService;
    private readonly IMemberService _memberService;
    private readonly IDataService _dataService;

    public AuthService(
        ILogger<AuthService> logger,
        IJwtService jwtService,
        IMemberService memberService,
        IDataService dataService
    )
    {
        _logger = logger;
        _jwtService = jwtService;
        _memberService = memberService;
        _dataService = dataService;
    }

    public async Task<SignInResponse> SignIn(SignInRequest request)
    {
        var emailOrPhone = request.PhoneOrEmail.Trim();
        var logPath = $"{emailOrPhone} {nameof(AuthService)} {nameof(SignIn)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (string.IsNullOrWhiteSpace(emailOrPhone))
        {
            throw new AirSoftBaseException(ErrorCodes.AuthService.EmptyLoginOrPass, "Пустой телефон или почта");
        }
        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new AirSoftBaseException(ErrorCodes.AuthService.EmptyPassword, "Пустой пароль");
        }
        var isEmail = EmailHelper.IsValidEmail(emailOrPhone);
        DbUser? dbUser = isEmail
            ? await _dataService.Users.GetByEmailAsync(emailOrPhone)
            : await _dataService.Users.GetByPhoneAsync(PhoneHelper.CleanPhone(emailOrPhone));

        if (dbUser == null)
        {
            throw new AirSoftBaseException(ErrorCodes.AuthService.UserNotFound, "Пользователь не найден");
        }
        var validPassword = dbUser?.ValidPassword(request.Password) ?? false;
        if (validPassword)
        {
            _logger.Log(LogLevel.Trace, $"{logPath}. Password valid.");
            var tokenData = await _jwtService.BuildToken(new JwtRequest(dbUser!));
            return new SignInResponse(tokenData, new UserData(
                dbUser!.Id,
                dbUser.Email,
                dbUser.Phone,
                dbUser.Status
            ));
        }

        throw new AirSoftBaseException(ErrorCodes.AuthService.WrongLoginOrPass, "Неверный логин или пароль");
    }

    public async Task<SignUpResponse> SignUp(SignUpRequest request)
    {
        var emailOrPhone = request.PhoneOrEmail.Trim();
        var logPath = $"{emailOrPhone} {nameof(AuthService)} {nameof(SignIn)}. | ";
        if (string.IsNullOrWhiteSpace(emailOrPhone))
        {
            throw new AirSoftBaseException(ErrorCodes.AuthService.EmptyLoginOrPass, "Пустой телефон или почта");
        }
        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new AirSoftBaseException(ErrorCodes.AuthService.EmptyPassword, "Пустой пароль");
        }
        if (!request.Password.Equals(request.ConfirmPassword))
        {
            throw new AirSoftBaseException(ErrorCodes.AuthService.PasswordsNotEqual, "Пароли не совпадают");
        }
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        var isEmail = EmailHelper.IsValidEmail(emailOrPhone);
        DbUser? dbUser = isEmail
            ? await _dataService.Users.GetByEmailAsync(emailOrPhone)
            : await _dataService.Users.GetByPhoneAsync(PhoneHelper.CleanPhone(emailOrPhone));

        if (dbUser != null)
        {
            throw new AirSoftBaseException(ErrorCodes.AuthService.AlreadyExist, "Пользователь с таким телефоном или почтой уже существует");
        }

        var dbUserRole = await _dataService.UserRoles!.GetAsync(x => x.Id == (int)UserRoleType.User);
        if (dbUserRole == null)
        {
            throw new AirSoftBaseException(ErrorCodes.AuthService.UserRoleNotFound, "Не найдена пользовательская роль");
        }

        var id = Guid.NewGuid();
        dbUser = new DbUser()
        {
            Id = id,
            Email = isEmail ? emailOrPhone : null,
            Phone = !isEmail ? PhoneHelper.CleanPhone(emailOrPhone) : null,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            CreatedBy = id,
            ModifiedBy = id,
            Status = UserStatus.New
        };
        dbUser.PasswordHash = dbUser.HashPassword(request.Password);
        dbUser.UsersToRoles = new List<DbUsersToRoles>()
        {
            new()
            {
                User = dbUser,
                Role = dbUserRole
            }
        };
        var created = this._dataService.Users.CreateDbUser(dbUser);
        if (created == null)
        {
            throw new AirSoftBaseException(ErrorCodes.AuthService.CreatedUserIsNull, "Созданный пользователь пустой");
        }
        _logger.Log(LogLevel.Information, $"{logPath} User created: {created!.Id}.");
        var member = await _memberService.Create(new CreateMemberRequest(created.Id));
        var tokenData = await _jwtService.BuildToken(new JwtRequest(created));

        await this._dataService.SaveAsync();
        return new SignUpResponse(tokenData, new UserData(created!.Id, created.Email, created.Phone, created.Status));
    }
}
