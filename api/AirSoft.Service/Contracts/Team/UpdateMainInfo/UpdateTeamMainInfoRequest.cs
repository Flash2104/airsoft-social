using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Team.UpdateMainInfo;

public class UpdateTeamMainInfoRequest
{
    public UpdateTeamMainInfoRequest(Guid id, string title, string? city, DateTime? foundationDate, ReferenceData<Guid>? leader)
    {
        Id = id;
        Title = title;
        City = city;
        FoundationDate = foundationDate;
        Leader = leader;
    }

    public Guid Id { get; }

    public string Title { get; set; }

    public string? City { get; }

    public DateTime? FoundationDate { get; }

    public ReferenceData<Guid>? Leader { get; }

}