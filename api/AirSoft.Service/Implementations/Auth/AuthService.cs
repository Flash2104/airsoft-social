﻿
using AirSoft.Data.Entity;
using AirSoft.Service.Common;
using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Auth;
using AirSoft.Service.Contracts.Auth.SignIn;
using AirSoft.Service.Contracts.Auth.SignUp;
using AirSoft.Service.Contracts.Jwt;
using AirSoft.Service.Contracts.Jwt.Model;
using AirSoft.Service.Contracts.Models;
using AirSoft.Service.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AirSoft.Service.Implementations.Auth;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly IJwtService _jwtService;
    private readonly IDataService _dataService;

    public AuthService(
        ILogger<AuthService> logger,
        IJwtService jwtService,
        IDataService dataService
    )
    {
        _logger = logger;
        _jwtService = jwtService;
        _dataService = dataService;
    }

    public async Task<SignInResponse> SignIn(SignInRequest request)
    {
        var emailOrPhone = request.PhoneOrEmail.Trim();
        var logPath = $"{emailOrPhone} {nameof(AuthService)} {nameof(SignIn)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        var isEmail = EmailHelper.IsValidEmail(emailOrPhone);
        DbUser? dbUser = isEmail
            ? _dataService.Users.GetByEmail(emailOrPhone)
            : _dataService.Users.GetByPhone(PhoneHelper.CleanPhone(emailOrPhone));

        //if (dbUser == null)
        //{
        //    throw new AirSoftBaseException(ErrorCodes.AuthService.UserNotFound, "Пользователь не найден");
        //}

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new AirSoftBaseException(ErrorCodes.AuthService.EmptyPassword, "Пустой пароль");
        }

        var validPassword = dbUser?.ValidPassword(request.Password) ?? false;
        if (validPassword)
        {
            _logger.Log(LogLevel.Trace, $"{logPath}. Password valid.");
            var tokenData = await _jwtService.BuildToken(new JwtRequest(dbUser));
            return new SignInResponse(tokenData, new UserData(
                dbUser.Id,
                dbUser.Email,
                dbUser.Phone
            ));
        }

        throw new AirSoftBaseException(ErrorCodes.AuthService.WrongLoginOrPass, "Неверный логин или пароль");
    }

    public Task<SignUpResponse> SignUp(SignUpRequest request)
    {
        throw new NotImplementedException();
    }
}