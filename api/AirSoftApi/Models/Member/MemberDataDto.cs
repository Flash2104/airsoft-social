using AirSoft.Service.Contracts.Models;

namespace AirSoftApi.Models.Member;

public class MemberDataDto
{
    public MemberDataDto(Guid id, string? name, string? surname, string? email, string? phone, byte[]? avatar, Guid? teamId, List<ReferenceData<int>>? roles)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Email = email;
        Phone = phone;
        AvatarData = avatar != null ? Convert.ToBase64String(avatar) : null;
        TeamId = teamId;
        Roles = roles;
    }

    public Guid Id { get; }

    public string? Name { get; set; }

    public string? Surname { get; }

    public string? Email { get; }

    public string? Phone { get; }

    public List<ReferenceData<int>>? Roles { get; }

    public string? AvatarData { get; }

    public Guid? TeamId { get; }
}