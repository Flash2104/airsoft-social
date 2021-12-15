using System.ComponentModel.DataAnnotations;
using AirSoft.Service.Contracts.Models;

namespace AirSoftApi.Models.TeamModify.UpdateMainInfo;

public class UpdateTeamMainInfoRequestDto: IValidatableObject
{
    public UpdateTeamMainInfoRequestDto(Guid id, string title, string? city, DateTime? foundationDate, ReferenceData<Guid>? leader)
    {
        Id = id;
        Title = title;
        City = city;
        FoundationDate = foundationDate;
        Leader = leader;
    }

    [Required]
    public Guid Id { get; }

    [Required]
    public string Title { get; set; }

    public string? City { get; }

    public DateTime? FoundationDate { get; }

    [Required]
    public ReferenceData<Guid>? Leader { get; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        throw new NotImplementedException();
    }
}