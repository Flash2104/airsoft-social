using System.ComponentModel.DataAnnotations;

namespace AirSoftApi.Models.Team.Create;

public class CreateTeamRequestDto
{
    public CreateTeamRequestDto(string title, string? city, DateTime? foundationDate, byte[]? avatar)
    {
        Title = title;
        City = city;
        FoundationDate = foundationDate;
        Avatar = avatar;
    }
    
    [Required]
    public string Title { get; set; }

    public string? City { get; }

    public DateTime? FoundationDate { get; }

    public byte[]? Avatar { get; }
}