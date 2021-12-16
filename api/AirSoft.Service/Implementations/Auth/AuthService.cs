
using AirSoft.Data.Entity;
using AirSoft.Service.Common;
using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Auth;
using AirSoft.Service.Contracts.Auth.SignIn;
using AirSoft.Service.Contracts.Auth.SignUp;
using AirSoft.Service.Contracts.Jwt;
using AirSoft.Service.Contracts.Jwt.Model;
using AirSoft.Service.Contracts.Models;
using AirSoft.Service.Contracts.User;
using AirSoft.Service.Contracts.User.Register;
using AirSoft.Service.Exceptions;
using Microsoft.Extensions.Logging;

namespace AirSoft.Service.Implementations.Auth;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly IJwtService _jwtService;
    private readonly IUserService _userService;
    private readonly IDataService _dataService;

    public AuthService(
        ILogger<AuthService> logger,
        IJwtService jwtService,
        IUserService userService,
        IDataService dataService
    )
    {
        _logger = logger;
        _jwtService = jwtService;
        _userService = userService;
        _dataService = dataService;
    }

    public async Task<SignInResponse> SignIn(SignInRequest request)
    {
        var emailOrPhone = request.PhoneOrEmail.Trim();
        var logPath = $"{emailOrPhone} {nameof(AuthService)} {nameof(SignIn)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        var getUser = await _userService.GetUserByEmailOrPhone(emailOrPhone);
        var dbUser = getUser.User;
        var validPassword = await _userService.ValidateUserPass(getUser.User.Id, request.Password);
        if (validPassword)
        {
            _logger.Log(LogLevel.Trace, $"{logPath}. Password valid.");
            var tokenData = await _jwtService.BuildToken(new JwtRequest(getUser.User!));
            return new SignInResponse(tokenData, new UserData(
                dbUser!.Id,
                dbUser.Email,
                dbUser.Phone,
                dbUser.Status
            ));
        }

        throw new AirSoftBaseException(ErrorCodes.AuthService.WrongLoginOrPass, "Неверный логин или пароль", logPath);
    }

    public async Task<SignUpResponse> SignUp(SignUpRequest request)
    {
        var emailOrPhone = request.PhoneOrEmail.Trim();
        var logPath = $"{emailOrPhone} {nameof(AuthService)} {nameof(SignIn)}. | ";
        if (string.IsNullOrWhiteSpace(emailOrPhone))
        {
            throw new AirSoftBaseException(ErrorCodes.AuthService.EmptyLoginOrPass, "Пустой телефон или почта", logPath);
        }

        var created = await _userService.RegisterUser(new RegisterUserRequest(request.PhoneOrEmail, request.Password,
                request.ConfirmPassword));
        var userData = created.User;
        var tokenData = await _jwtService.BuildToken(new JwtRequest(userData));

        return new SignUpResponse(tokenData, new UserData(userData!.Id, userData.Email, userData.Phone, userData.Status));
    }
}
