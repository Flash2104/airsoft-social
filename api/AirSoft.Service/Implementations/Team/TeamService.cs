using AirSoft.Data.Entity;
using AirSoft.Service.Common;
using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Models;
using AirSoft.Service.Contracts.Team;
using AirSoft.Service.Contracts.Team.Create;
using AirSoft.Service.Contracts.Team.Delete;
using AirSoft.Service.Contracts.Team.Get;
using AirSoft.Service.Contracts.Team.GetCurrent;
using AirSoft.Service.Contracts.Team.UpdateMainInfo;
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

        //if (dbTeam == null)
        //{
        //    throw new AirSoftBaseException(ErrorCodes.TeamService.NotFound, "Команда пользователя не найдена");
        //}

        return new GetCurrentTeamResponse(MapToTeamData(dbTeam));
    }


    public Task<GetByIdTeamResponse> Get(GetByIdTeamRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<CreateTeamResponse> Create(CreateTeamRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<UpdateTeamMainInfoResponse> UpdateMainInfo(UpdateTeamMainInfoRequest request)
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
        dbTeam.FoundationDate = request.FoundationDate;
        dbTeam.City = request.City;
        if (request.Leader != null && request.Leader.Id != Guid.Empty)
        {
            if (dbTeam.Members!.All(x => x.UserId != request.Leader.Id))
            {
                throw new AirSoftBaseException(ErrorCodes.TeamService.LeaderNotInTeam, "Командир не является членом команды");
            }
            dbTeam.LeaderId = request.Leader.Id;
        }
        dbTeam.Title = request.Title;

        this._dataService.Team.Update(dbTeam);

        _logger.Log(LogLevel.Information, $"{logPath} Team Main info updated: {dbTeam.Id}.");
        return new UpdateTeamMainInfoResponse(MapToTeamData(dbTeam));
    }

    public Task Delete(DeleteTeamRequest request)
    {
        throw new NotImplementedException();
    }
    
    private TeamData? MapToTeamData(DbTeam? dbTeam)
    {
        return dbTeam != null ? new TeamData(
            dbTeam.Id,
            dbTeam.Title,
            dbTeam.City,
            dbTeam.FoundationDate,
            dbTeam.Avatar,
            dbTeam.Members?
                .Select(x => new MemberViewData(
                    x.Id,
                    x.Name,
                    x.Surname,
                    x.City,
                    x.About,
                    x.Avatar,
                    x.Id == dbTeam.LeaderId,
                    x.TeamMemberRoles?.Select(y => new ReferenceData<Guid>(
                        y.Id,
                        y.Title ?? throw new AirSoftBaseException(ErrorCodes.TeamService.EmptyRoleTitle, "Пустое имя роли члена команды"),
                        y.Rank)
                    ).ToList()))
                .ToList(),
            dbTeam.TeamRoles?.Select(x => new ReferenceData<Guid>(x.Id, x.Title, x.Rank)).ToList()
        ) : null;
    }
}