using System.ComponentModel.DataAnnotations;

namespace AirSoftApi.Models.Member.Get;

public class GetMemberRequestDto : IValidatableObject
{
    public GetMemberRequestDto(string id)
    {
        Id = id;
    }

    [Required]
    public string Id { get; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return new List<ValidationResult>();
    }
}