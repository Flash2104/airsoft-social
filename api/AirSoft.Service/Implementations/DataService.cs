using AirSoft.Data;
using AirSoft.Service.Contracts;
using AirSoft.Service.Repositories;

namespace AirSoft.Service.Implementations;

public class DataService: IDataService
{
    private readonly IDbContext _dbContext;

    private UserRepository? _users;

    public DataService(IDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public UserRepository Users => _users ??= new UserRepository(_dbContext);
}