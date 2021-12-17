using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Member.Update;

public class UpdateMemberRequest
{
    public UpdateMemberRequest(Guid id, string? name, string? surname, DateTime? birthDate, string? city)
    {
        Id = id;
        Name = name;
        Surname = surname;
        BirthDate = birthDate;
        City = city;
    }

    public Guid Id { get; }

    public string? Name { get; set; }

    public string? Surname { get; }

    public DateTime? BirthDate { get; }

    public string? City { get; }
    
}