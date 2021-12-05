using System.ComponentModel.DataAnnotations;

namespace AirSoftApi.Models.Profile.Get;

public class GetProfileRequestDto : IValidatableObject
{
    public GetProfileRequestDto()
    {
    }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return new List<ValidationResult>();
    }
}