using AirSoft.Service.Contracts.Member.GetCurrent;

namespace AirSoft.Service.Contracts.Member;

public interface IMemberService
{
    Task<GetCurrentMemberResponse> GetCurrentMember();
}