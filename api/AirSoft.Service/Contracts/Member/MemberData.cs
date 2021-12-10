using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Member;

public class MemberData
{
    public MemberData(
        Guid id,
        string? name,
        string? surname,
        DateTime? birthDate,
        string? city,
        string? email,
        string? phone,
        byte[]? avatar,
        ReferenceData<Guid>? team,
        List<ReferenceData<Guid>>? roles
        )
    {
        Id = id;
        Name = name;
        Surname = surname;
        Email = email;
        Phone = phone;
        Avatar = avatar;
        Team = team;
        Roles = roles;
        BirthDate = birthDate;
        City = city;
    }

    public Guid Id { get; }

    public string? Name { get; set; }

    public string? Surname { get; }

    public string? Email { get; }

    public string? Phone { get; }

    public DateTime? BirthDate { get; }

    public string? City { get; }

    public byte[]? Avatar { get; }

    public ReferenceData<Guid>? Team { get; }

    public List<ReferenceData<Guid>>? Roles { get; }
}