using AirSoft.Service.Contracts.Member;

namespace AirSoft.Service.Contracts.Team.GetCurrent;

public class GetCurrentTeamResponse
{
    public GetCurrentTeamResponse(MemberData memberData)
    {
        MemberData = memberData;
    }
    
    public MemberData MemberData { get; }
}