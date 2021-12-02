using AirSoft.Data;
using AirSoft.Data.Entity;
using AirSoft.Service.Common;
using AirSoft.Service.Exceptions;

namespace AirSoft.Service.Repositories;

public class UserRepository: GenericRepository<DbUser>
{
    public UserRepository(IDbContext context) : base(context)
    {
    }

    public DbUser? GetByPhone(string phone)
    {
        var users = Get(e => e.Phone == phone);
        var dbUsers = users.ToList();
        if (dbUsers.Count > 1)
        {
            throw new AirSoftBaseException(ErrorCodes.UserRepository.MoreThanOneUserByPhone,
                "В базе больше одного пользователя по данному номеру телефона.");
        }

        return dbUsers.FirstOrDefault();
    }

    public DbUser? GetByEmail(string email)
    {
        var users = Get(e => e.Email == email);
        var dbUsers = users.ToList();
        if (dbUsers.Count > 1)
        {
            throw new AirSoftBaseException(ErrorCodes.UserRepository.MoreThanOneUserByPhone,
                "В базе больше одного пользователя по данному номеру телефона.");
        }

        return dbUsers.FirstOrDefault();
    }
}