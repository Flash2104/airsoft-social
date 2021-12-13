using AirSoft.Data.Entity;
using AirSoft.Service.Common;
using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Models;
using AirSoft.Service.Contracts.Team;
using AirSoft.Service.Contracts.Team.Create;
using AirSoft.Service.Contracts.Team.Delete;
using AirSoft.Service.Contracts.Team.Get;
using AirSoft.Service.Contracts.Team.GetCurrent;
using AirSoft.Service.Contracts.Team.Update;
using AirSoft.Service.Exceptions;
using Microsoft.Extensions.Logging;

namespace AirSoft.Service.Implementations.Team;

public class TeamService : ITeamService
{
    private readonly ILogger<TeamService> _logger;
    private readonly ICorrelationService _correlationService;
    private readonly IDataService _dataService;

    public TeamService(ILogger<TeamService> logger, ICorrelationService correlationService, IDataService dataService)
    {
        _logger = logger;
        _correlationService = correlationService;
        _dataService = dataService;
    }

    public async Task<GetCurrentTeamResponse> GetCurrent()
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(TeamService)} {nameof(GetCurrent)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (!userId.HasValue)
        {
            throw new AirSoftBaseException(ErrorCodes.TeamService.EmptyUserId, "Пустой идентификатор пользователя");
        }
        DbTeam? dbTeam = await _dataService.Team.GetByUserAsync(userId.GetValueOrDefault());

        if (dbTeam == null)
        {
            throw new AirSoftBaseException(ErrorCodes.TeamService.NotFound, "Команда не найдена");
        }

        return new GetCurrentTeamResponse(new TeamData(
            dbTeam.Id,
            dbTeam.Title,
            dbTeam.City,
            dbTeam.Avatar,
            dbTeam.Members?
                .Select(x => new MemberViewData(
                    x.Id,
                    x.Name,
                    x.Surname,
                    x.City,
                    x.About,
                    x.Avatar,
                    x.IsTeamLeader,
                    x.TeamMemberRoles?.Select(y => new ReferenceData<Guid>(
                        y.Id,
                        y.Title ?? throw new AirSoftBaseException(ErrorCodes.TeamService.EmptyRoleTitle, "Пустое имя роли члена команды"),
                        y.Rank)
                    ).ToList()))
                .ToList(),
            dbTeam.TeamRoles?.Select(x => new ReferenceData<Guid>(x.Id, x.Title, x.Rank)).ToList()
        ));
    }

    public Task<GetByIdTeamResponse> Get(GetByIdTeamRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<CreateTeamResponse> Create(CreateTeamRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<UpdateTeamResponse> Update(UpdateTeamRequest request)
    {
        throw new NotImplementedException();
    }

    public Task Delete(DeleteTeamRequest request)
    {
        throw new NotImplementedException();
    }
}