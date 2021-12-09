using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Team;

public class TeamData
{
    public TeamData(Guid id, string? title, string? city, byte[]? avatar, List<MemberViewData> members)
    {
        Id = id;
        Title = title;
        Avatar = avatar;
        Members = members;
        City = city;
    }

    public Guid Id { get; }

    public string? Title { get; set; }

    public string? City { get; }

    public byte[]? Avatar { get; }

    public List<MemberViewData> Members { get; }
}

public class MemberViewData
{
    public MemberViewData(Guid id, string? name, string? surname, string? city, byte[]? avatar, List<ReferenceData<int>>? roles)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Avatar = avatar;
        Roles = roles;
        City = city;
    }

    public Guid Id { get; }

    public string? Name { get; set; }

    public string? Surname { get; }
    
    public string? City { get; }

    public byte[]? Avatar { get; }

    public List<ReferenceData<int>>? Roles { get; }
}