
using AirSoft.Data.Entity;

namespace AirSoft.Service.Contracts.Jwt.Model;

public class JwtRequest
{
    public JwtRequest(DbUser user)
    {
        User = user;
    }

    public DbUser User { get; }
}

