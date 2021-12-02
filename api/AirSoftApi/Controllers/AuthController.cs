using AirSoft.Service.Contracts.Auth;
using AirSoft.Service.Contracts.Auth.SignIn;
using AirSoftApi.Models;
using AirSoftApi.Models.Auth;
using AirSoftApi.Models.Auth.SignIn;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirSoftApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : RootController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService) : base(logger)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        public async Task<ServerResponseDto<SignInResponseDto>> SignIn([FromBody] SignInRequestDto request)
        {
            var logPath = $"{request.PhoneOrEmail}.{nameof(AuthController)} {nameof(SignIn)} | ";
            return await HandleRequest<SignInRequestDto, SignInRequest, SignInResponse, SignInResponseDto>(
                _authService.SignIn,
                request,
                dto => new SignInRequest(dto.PhoneOrEmail, dto.Password),
                res => new SignInResponseDto(
                    new TokenResponseDto(res.TokenData.Token, res.TokenData.ExpiryDate),
                    new UserDto(res.User.Id, res.User.Email, res.User.Phone)),
                logPath
            );
        }
    }
}
