using AirSoft.Data.Entity;

namespace AirSoft.Service.Contracts.Models;

public class UserData
{
    public UserData(Guid id, string? email, string? phone, UserStatus? status)
    {
        Id = id;
        Email = email;
        Phone = phone;
        Status = status;
    }

    public Guid Id { get; }

    public string? Email { get; }

    public string? Phone { get; }

    public UserStatus? Status { get; }
}