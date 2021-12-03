using AirSoft.Service.Contracts.Models;

namespace AirSoftApi.Models.Auth.SignUp;

public class SignUpResponseDto
{
    public SignUpResponseDto(TokenResponseDto tokenData, UserDto user)
    {
        TokenData = tokenData;
        User = user;
    }

    public TokenResponseDto TokenData { get; }

    public UserDto User { get; }
}