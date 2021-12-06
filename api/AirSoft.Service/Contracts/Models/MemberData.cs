namespace AirSoft.Service.Contracts.Models;

public class MemberData
{
    public MemberData(Guid id, string? name, string? surname, string? email, string? phone, byte[]? avatar, Guid? teamId, List<ReferenceData<int>>? roles)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Email = email;
        Phone = phone;
        Avatar = avatar;
        TeamId = teamId;
        Roles = roles;
    }

    public Guid Id { get; }

    public string? Name { get; set; }

    public string? Surname { get; }

    public string? Email { get; }

    public string? Phone { get; }

    public byte[]? Avatar { get; }

    public Guid? TeamId { get; }

    public List<ReferenceData<int>>? Roles { get; }
}