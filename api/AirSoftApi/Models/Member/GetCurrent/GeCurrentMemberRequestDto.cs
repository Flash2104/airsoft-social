using System.ComponentModel.DataAnnotations;

namespace AirSoftApi.Models.Member.GetCurrent;

public class GetCurrentMemberRequestDto : IValidatableObject
{
    public GetCurrentMemberRequestDto()
    {
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return new List<ValidationResult>();
    }
}