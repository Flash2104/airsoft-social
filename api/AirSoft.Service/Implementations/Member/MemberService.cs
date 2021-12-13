using System.Reflection;
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
            throw new AirSoftBaseException(ErrorCodes.MemberService.NotFound, "Профиль не найден");
        }

        return new GetCurrentMemberResponse(new MemberData(
            dbMember.Id,
            dbMember.Name,
            dbMember.Surname,
            dbMember.BirthDate,
            dbMember.City,
            dbMember.User?.Email,
            dbMember.User?.Phone,
            dbMember.Avatar?.ToArray(),
            dbMember.Team != null ? new ReferenceData<Guid>(dbMember.Team.Id, dbMember.Team.Title) : null,
            dbMember.TeamMemberRoles?.Select(x =>
                new ReferenceData<Guid>(
                    x.Id,
                    x.Title ?? throw new AirSoftBaseException(ErrorCodes.CommonError, "Пустое имя роли члена команды"),
                    x.Rank))
                .ToList()
        ));
    }

    public Task<GetByIdMemberResponse> Get(GetByIdMemberRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<CreateMemberResponse> Create(CreateMemberRequest request)
    {
        var logPath = $"{request.UserId} {nameof(MemberService)} {nameof(Create)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");

        DbMember? dbMember = await _dataService.Member.GetByUserAsync(request.UserId);
        if (dbMember != null)
        {
            throw new AirSoftBaseException(ErrorCodes.MemberService.AlreadyExist, "Профиль уже существует");
        }
        string? root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        dbMember = new DbMember()
        {
            Id = Guid.NewGuid(),
            Name = request.Name ?? "Новенький",
            Surname = request.Surname,
            City = request.City,
            CreatedBy = request.UserId,
            ModifiedBy = request.UserId,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            UserId = request.UserId,
            Avatar = await File.ReadAllBytesAsync(root + "\\InitialData\\member-default.png")
        };
        var created = this._dataService.Member.Insert(dbMember);
        if (created == null)
        {
            throw new AirSoftBaseException(ErrorCodes.AuthService.CreatedUserIsNull, "Созданный пользователь пустой");
        }

        _logger.Log(LogLevel.Information, $"{logPath} Member created: {created!.Id}.");
        return new CreateMemberResponse(new MemberData(
            created.Id,
            created.Name,
            created.Surname,
            created.BirthDate,
            created.City,
            created.User?.Email,
            created.User?.Phone,
            created.Avatar,
            null,
            null
            ));
    }

    public async Task<UpdateMemberResponse> Update(UpdateMemberRequest request)
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(MemberService)} {nameof(Update)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (!userId.HasValue)
        {
            throw new AirSoftBaseException(ErrorCodes.MemberService.EmptyUserId, "Пустой идентификатор пользователя");
        }
        DbMember? dbMember = await _dataService.Member.GetAsync(x => x.UserId == userId && x.Id == request.Id);

        if (dbMember == null)
        {
            throw new AirSoftBaseException(ErrorCodes.MemberService.NotFound, "Профиль не найден");
        }
        dbMember.Avatar = request.Avatar?.ToArray();
        dbMember.BirthDate = request.BirthDate;
        dbMember.City = request.City;
        dbMember.Name = request.Name;
        dbMember.Surname = request.Surname;
        dbMember.TeamId = request.Team?.Id;

        this._dataService.Member.Update(dbMember);

        _logger.Log(LogLevel.Information, $"{logPath} Member updated: {dbMember!.Id}.");
        return new UpdateMemberResponse(new MemberData(
            dbMember.Id,
            dbMember.Name,
            dbMember.Surname,
            dbMember.BirthDate,
            dbMember.City,
            dbMember.User?.Email,
            dbMember.User?.Phone,
            dbMember.Avatar?.ToArray(),
            dbMember.Team != null ? new ReferenceData<Guid>(dbMember.Team.Id, dbMember.Team.Title) : null,
            dbMember.TeamMemberRoles?.Select(x => new ReferenceData<Guid>(x.Id, x.Title, x.Rank)).ToList()
        ));
    }

    public async Task Delete(DeleteMemberRequest request)
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(MemberService)} {nameof(Update)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (!userId.HasValue)
        {
            throw new AirSoftBaseException(ErrorCodes.MemberService.EmptyUserId, "Пустой идентификатор пользователя");
        }
        DbMember? dbMember = await _dataService.Member.GetAsync(x => x.UserId == userId && x.Id == request.Id);

        if (dbMember == null)
        {
            throw new AirSoftBaseException(ErrorCodes.MemberService.NotFound, "Профиль не найден");
        }

        this._dataService.Member.Delete(dbMember);

        _logger.Log(LogLevel.Information, $"{logPath} Member deleted: {request!.Id}.");
    }
}