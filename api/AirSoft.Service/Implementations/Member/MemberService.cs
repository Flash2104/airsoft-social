using AirSoft.Data.Entity;
using AirSoft.Service.Common;
using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Member;
using AirSoft.Service.Contracts.Member.Create;
using AirSoft.Service.Contracts.Member.Delete;
using AirSoft.Service.Contracts.Member.Get;
using AirSoft.Service.Contracts.Member.GetCurrent;
using AirSoft.Service.Contracts.Member.Update;
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

    public async Task<GetCurrentMemberResponse> GetCurrent()
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(MemberService)} {nameof(GetCurrent)}. | ";
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
            dbMember.BirthDate,
            dbMember.City,
            dbMember.User?.Email,
            dbMember.User?.Phone,
            dbMember.Avatar.ToArray(),
            dbMember.Team != null ? new ReferenceData<Guid>(dbMember.Team.Id, dbMember.Team.Title) : null,
            dbMember.MemberRoles?.Select(x => new ReferenceData<int>(x.Id, x.Role)).ToList()
        ));
    }

    public Task<GetByIdMemberResponse> Get(GetByIdMemberRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<CreateMemberResponse> Create(CreateMemberRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<UpdateMemberResponse> Update(UpdateMemberRequest request)
    {
        throw new NotImplementedException();
    }

    public Task Delete(DeleteMemberRequest request)
    {
        throw new NotImplementedException();
    }
}