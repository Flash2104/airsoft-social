namespace AirSoftApi.Models.Member;

public class MemberDataDto
{
    public MemberDataDto(Guid id, string? name, string? surname, string? email, string? phone, byte[]? avatar, Guid? teamId)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Email = email;
        Phone = phone;
        Avatar = avatar;
        TeamId = teamId;
    }

    public Guid Id { get; }

    public string? Name { get; set; }

    public string? Surname { get; }

    public string? Email { get; }

    public string? Phone { get; }

    public byte[]? Avatar { get; }

    public Guid? TeamId { get; }
}