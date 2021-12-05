using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Member.Get;

public class MemberGetResponse
{
    public MemberGetResponse(MemberData memberData)
    {
        MemberData = memberData;
    }
    
    public MemberData MemberData { get; }
}