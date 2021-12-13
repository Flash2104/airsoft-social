using AirSoft.Data.Entity;
using AirSoft.Service.Common;
using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Models;
using AirSoft.Service.Contracts.Navigation;
using AirSoft.Service.Contracts.Team;
using AirSoft.Service.Contracts.Team.GetCurrent;
using AirSoft.Service.Exceptions;
using Microsoft.Extensions.Logging;

namespace AirSoft.Service.Implementations;

public class NavigationService : INavigationService
{
    private readonly ILogger<NavigationService> _logger;
    private readonly ICorrelationService _correlationService;
    private readonly IDataService _dataService;

    public NavigationService(
        ILogger<NavigationService> logger,
        ICorrelationService correlationService,
        IDataService dataService)
    {
        _logger = logger;
        _correlationService = correlationService;
        _dataService = dataService;
    }

    public async Task<UserNavigationDataResponse> GetUserNavigations()
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(NavigationService)} {nameof(GetUserNavigations)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (!userId.HasValue)
        {
            throw new AirSoftBaseException(ErrorCodes.NavigationService.EmptyUserId, "Пустой идентификатор пользователя");
        }
        List<DbUserRole> dbUserRoles = await _dataService.Users.GetRolesWithNavigationsAsync(userId.GetValueOrDefault());

        if (dbUserRoles == null)
        {
            throw new AirSoftBaseException(ErrorCodes.NavigationService.UserRolesNotFound, "Не найдены роли пользователя");
        }

        return MapTreeToResponse(dbUserRoles!);
    }

    private UserNavigationDataResponse MapTreeToResponse(List<DbUserRole?> dbRoles)
    {
        var data = new List<RolesNavigationData>();
        bool containsTeamLead = dbRoles.Any(x => x.Id == (int)UserRoleType.TeamLeader);
        foreach (var dbRole in dbRoles)
        {
            if (containsTeamLead && dbRole.Id == (int)UserRoleType.Player)
            {
                continue;
            }
            var navTree = new Dictionary<int, List<NavigationItem>>()
            {
                {0, new List<NavigationItem>()}
            };
            if (dbRole?.UserNavigation == null || dbRole.UserNavigation!.NavigationItems == null)
            {
                continue; // ToDO: throw new AirSoftBaseException(ErrorCodes.NavigationService.NavigationNotFound, "Навигация для роли пользователя не найдена");
            }
            foreach (var dbNavItem in dbRole.UserNavigation!.NavigationItems)
            {
                var parentId = dbNavItem.ParentId ?? 0;
                if (!navTree.ContainsKey(dbNavItem.Id))
                {
                    navTree[dbNavItem.Id] = new List<NavigationItem>();
                }
                var item = new NavigationItem(dbNavItem.Id, dbNavItem.Path, dbNavItem.Title, dbNavItem.Icon,
                    dbNavItem.Order, navTree[dbNavItem.Id]);
                navTree[parentId].Add(item);
            }
            data.Add(new RolesNavigationData(new ReferenceData<int>(dbRole.Id, dbRole.Role), navTree[0]));
        }

        return new UserNavigationDataResponse(data);
    }
}