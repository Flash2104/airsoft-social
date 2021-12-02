using AirSoft.Service.Contracts.Jwt.Model;

namespace AirSoft.Service.Contracts.Auth.SignUp;

public class SignUpResponse
{
    public SignUpResponse(JwtResponse tokenData)
    {
        TokenData = tokenData;
    }

    public JwtResponse TokenData { get; }
}