using AirSoft.Service.Contracts.Member;

namespace AirSoft.Service.Contracts.Team.Create;

public class CreateTeamResponse
{
    public CreateTeamResponse(MemberData memberData)
    {
        MemberData = memberData;
    }
    
    public MemberData MemberData { get; }
}