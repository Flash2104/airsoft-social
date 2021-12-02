namespace AirSoftApi.Models.Auth.SignUp;

public class SignUpResponseDto
{
    public SignUpResponseDto(TokenResponseDto tokenData)
    {
        TokenData = tokenData;
    }

    public TokenResponseDto TokenData { get; }
}