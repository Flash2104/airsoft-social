using AirSoft.Service.Contracts.Jwt.Model;
using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Auth.SignIn;

public class SignInResponse
{
    public SignInResponse(JwtResponse tokenData, UserData user)
    {
        TokenData = tokenData;
        User = user;
    }

    public JwtResponse TokenData { get; }
    public UserData User { get; }
}