using AirSoft.Data.Entity;
using AirSoft.Service.Common;
using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Member;
using AirSoft.Service.Contracts.Member.GetCurrent;
using AirSoft.Service.Contracts.Models;
using AirSoft.Service.Exceptions;
using Microsoft.Extensions.Logging;

namespace AirSoft.Service.Implementations.Member;

public class MemberService : IMemberService
{
    private readonly ILogger<MemberService> _logger;
    private readonly ICorrelationService _correlationService;
    private readonly IDataService _dataService;

    public MemberService(ILogger<MemberService> logger, ICorrelationService correlationService, IDataService dataService)
    {
        _logger = logger;
        _correlationService = correlationService;
        _dataService = dataService;
    }

    public async Task<GetCurrentMemberResponse> GetCurrentMember()
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(MemberService)} {nameof(GetCurrentMember)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (!userId.HasValue)
        {
            throw new AirSoftBaseException(ErrorCodes.MemberService.EmptyUserId, "Пустой идентификатор пользователя");
        }
        DbMember? dbMember = await _dataService.Member.GetByUserAsync(userId.GetValueOrDefault());

        if (dbMember == null)
        {
            throw new AirSoftBaseException(ErrorCodes.MemberService.UserNotFound, "Профиль не найден");
        }

        return new GetCurrentMemberResponse(new MemberData(
            dbMember.Id,
            dbMember.Name,
            dbMember.Surname,
            dbMember.User?.Email,
            dbMember.User?.Phone,
            dbMember.Avatar.ToArray(),
            dbMember.TeamId
        ));
    }
}