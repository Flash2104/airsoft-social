using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Member.Get;

public class GetByIdMemberRequest
{
    public GetByIdMemberRequest(string id)
    {
        Id = id;
    }
    
    public string Id { get; }
}