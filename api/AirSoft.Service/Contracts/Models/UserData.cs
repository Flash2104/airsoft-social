namespace AirSoft.Service.Contracts.Models;

public class UserData
{
    public UserData(Guid id, string? email, string? phone)
    {
        Id = id;
        Email = email;
        Phone = phone;
    }

    public Guid Id { get; }

    public string? Email { get; }

    public string? Phone { get; }
}