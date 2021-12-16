using AirSoft.Data;
using AirSoft.Data.Entity;
using AirSoft.Service.Common;
using AirSoft.Service.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AirSoft.Service.Repositories;

public class TeamRepository : GenericRepository<DbTeam>
{
    private readonly DbSet<DbMember> _dbMembers;
    private readonly DbSet<DbTeamRole> _dbTeamRoles;

    public TeamRepository(IDbContext context) : base(context)
    {
        _dbMembers = context.Set<DbMember>();
        _dbTeamRoles = context.Set<DbTeamRole>();
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

    public async Task<DbTeam> Create(DbTeam dbTeam)
    {
        var created = Insert(dbTeam);
        if (created == null || created.Id == Guid.Empty)
        {
            throw new AirSoftBaseException(ErrorCodes.TeamRepository.CreatedTeamIsNull, "Результат создания команды пустой или отсутствует идентификатор");
        }
        var teamRoleIds = new Dictionary<int, Guid>(Enum.GetValues<DefaultMemberRoleType>().Select(x => new KeyValuePair<int, Guid>((int)x, Guid.NewGuid())));
        await CreateDefaultRolesAsync(created.Id, teamRoleIds);
        await AddLeaderToTeamMembers(created.LeaderId, created.Id, teamRoleIds);
        return created;
    }

    private async Task AddLeaderToTeamMembers(Guid memberId, Guid teamId, Dictionary<int, Guid> teamRoleIds)
    {
        var dbMember = await _dbMembers.FirstOrDefaultAsync(x => x.Id == memberId).ConfigureAwait(false);
        if (dbMember == null)
        {
            throw new AirSoftBaseException(ErrorCodes.TeamRepository.MemberNotFound, "Профиль пользователя не найден");
        }

        dbMember.TeamId = teamId;
        dbMember.TeamRolesToMembers = new List<DbTeamRolesToMembers>()
        {
            new DbTeamRolesToMembers()
            {
                MemberId = memberId,
                TeamRoleId = teamRoleIds[(int)DefaultMemberRoleType.Командир]
            }
        };
        _dbMembers.Update(dbMember);
    }

    private async Task CreateDefaultRolesAsync(Guid teamId, Dictionary<int, Guid> teamRoleIds)
    {
        var hasRoles = await _dbTeamRoles.AnyAsync(x => x.TeamId == teamId);
        if (hasRoles)
        {
            throw new AirSoftBaseException(ErrorCodes.TeamRolesRepository.AlreadyHasRoles, "У команды уже есть роли");
        }

        var roles = Enum.GetValues<DefaultMemberRoleType>().Select(v => new DbTeamRole
        {
            Id = teamRoleIds[(int)v],
            Title = v.ToString(),
            CreatedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            ModifiedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            Rank = (int)v,
            TeamId = teamId
        })
            .ToArray();
        foreach (var role in roles)
        {
            _dbTeamRoles.Add(role);
        }
    }
}