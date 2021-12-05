using AirSoft.Service.Contracts.Member.Get;

namespace AirSoft.Service.Contracts.Member;

public interface IMemberService
{
    Task<MemberGetResponse> GetMember();
}