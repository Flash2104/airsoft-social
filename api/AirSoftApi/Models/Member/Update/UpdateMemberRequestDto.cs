using AirSoft.Service.Contracts.Models;

namespace AirSoftApi.Models.Member.Update;

public class UpdateMemberRequestDto
{
    public UpdateMemberRequestDto(Guid id, string? name, string? surname, DateTime? birthDate, string? city, byte[]? avatar, ReferenceData<Guid>? team)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Avatar = avatar;
        Team = team;
        BirthDate = birthDate;
        City = city;
    }

    public Guid Id { get; }

    public string? Name { get; set; }

    public string? Surname { get; }

    public DateTime? BirthDate { get; }

    public string? City { get; }

    public byte[]? Avatar { get; }

    public ReferenceData<Guid>? Team { get; }
}