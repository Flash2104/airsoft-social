using System.ComponentModel.DataAnnotations;

namespace AirSoftApi.Models.Profile.Get;

public class GetProfileResponseDto
{
    public GetProfileResponseDto()
    {
    }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return new List<ValidationResult>();
    }
}