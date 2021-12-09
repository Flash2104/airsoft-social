using AirSoft.Data;
using AirSoft.Data.Entity;
using AirSoft.Service.Common;
using AirSoft.Service.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AirSoft.Service.Repositories;

public class TeamRepository: GenericRepository<DbTeam>
{
    private readonly DbSet<DbMember> _dbMembers;

    public TeamRepository(IDbContext context) : base(context)
    {
        _dbMembers = context.Set<DbMember>();
    }

    public async Task<DbTeam?> GetByUserAsync(Guid userId)
    {
        var dbMember = await _dbMembers.FirstOrDefaultAsync(x => x.UserId == userId).ConfigureAwait(false);
        if (dbMember == null)
        {
            throw new AirSoftBaseException(ErrorCodes.TeamRepository.MemberNotFound, "Профиль пользователя не найден");
        }
        return dbMember.Team;
    }
}