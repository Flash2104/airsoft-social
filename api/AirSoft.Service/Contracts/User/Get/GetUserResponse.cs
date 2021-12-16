using AirSoft.Data.Entity;

namespace AirSoft.Service.Contracts.User.Get;

public class GetUserResponse
{
    public GetUserResponse(DbUser user)
    {
        User = user;
    }

    public DbUser User { get; }
}