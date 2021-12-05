using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Member;
using AirSoft.Service.Contracts.Member.Get;

namespace AirSoft.Service.Implementations.Member;

public class MemberService : IMemberService
{
    private readonly ICorrelationService _correlationService;

    public MemberService(ICorrelationService correlationService)
    {
        _correlationService = correlationService;
    }

    public async Task<MemberGetResponse> GetMember()
    {
        var userId = _correlationService.GetUserId();

    }
}