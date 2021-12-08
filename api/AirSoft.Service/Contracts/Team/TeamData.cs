namespace AirSoft.Service.Contracts.Team;

public class TeamData
{
    public TeamData(Guid id, string? title, string? city, byte[]? avatar)
    {
        Id = id;
        Title = title;
        Avatar = avatar;
        City = city;
    }

    public Guid Id { get; }

    public string? Title { get; set; }

    public string? City { get; }

    public byte[]? Avatar { get; }
}