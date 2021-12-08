using AirSoft.Service.Contracts.Member;

namespace AirSoft.Service.Contracts.Team.Update;

public class UpdateTeamResponse
{
    public UpdateTeamResponse(MemberData memberData)
    {
        MemberData = memberData;
    }
    
    public MemberData MemberData { get; }
}