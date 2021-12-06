using AirSoft.Data;
using AirSoft.Data.Entity;
using AirSoft.Service.Contracts;
using AirSoft.Service.Repositories;

namespace AirSoft.Service.Implementations;

public class DataService : IDataService
{
    private readonly IDbContext _dbContext;

    private UserRepository? _users;
    private MemberRepository? _members;
    private GenericRepository<DbUserRole>? _userRoles;
    private GenericRepository<DbMemberRole>? _memberRoles;

    public DataService(IDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public UserRepository Users => _users ??= new UserRepository(_dbContext);

    public MemberRepository Member => _members ??= new MemberRepository(_dbContext);

    public GenericRepository<DbUserRole>? UserRoles => _userRoles ??= new GenericRepository<DbUserRole>(_dbContext);

    public GenericRepository<DbMemberRole>? MemberRoles => _memberRoles ??= new GenericRepository<DbMemberRole>(_dbContext);

    public async Task<bool> SaveAsync()
    {
        await _dbContext.SaveAsync().ConfigureAwait(false);
        return true;
    }
}