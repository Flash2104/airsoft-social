using AirSoft.Data.Entity;

namespace AirSoft.Service.Contracts.User.Register;

public class RegisterUserResponse
{
    public RegisterUserResponse(DbUser user)
    {
        User = user;
    }

    public DbUser User { get; }
}