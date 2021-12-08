using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Team.Update;

public class UpdateTeamRequest
{
    public UpdateTeamRequest(Guid id, string? title, string? city, byte[]? avatar)
    {
        Id = id;
        Avatar = avatar;
        City = city;
    }

    public Guid Id { get; }

    public string? Name { get; set; }

    public string? City { get; }

    public byte[]? Avatar { get; }
    
}