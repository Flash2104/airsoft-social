using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Team.UpdateMainInfo;

public class UpdateTeamMainInfoRequest
{
    public UpdateTeamMainInfoRequest(Guid id, string title, int? cityId, DateTime? foundationDate, ReferenceData<Guid>? leader)
    {
        Id = id;
        Title = title;
        CityId = cityId;
        FoundationDate = foundationDate;
        Leader = leader;
    }

    public Guid Id { get; }

    public string Title { get; set; }

    public int? CityId { get; }

    public DateTime? FoundationDate { get; }

    public ReferenceData<Guid>? Leader { get; }

}