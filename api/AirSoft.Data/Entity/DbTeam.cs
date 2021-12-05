using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirSoft.Data.Entity;

public class DbTeam : DbEntity<Guid>
{
    public DbTeam()
    {
    }
    private DbTeam(ILazyLoader lazyLoader)
    {
        LazyLoader = lazyLoader;
    }

    private ILazyLoader LazyLoader { get; set; } = null!;
    
    private List<DbMember>? _members;
    public string Title { get; set; } = null!;

    public List<DbMember>? Members
    {
        get => LazyLoader.Load(this, ref _members);
        set => _members = value;
    }

    public DbMember? Leader => Members?.FirstOrDefault(x => x.MemberRoles?.Select(y => y.Id).Contains((int)MemberRoleType.Leader) ?? false);
}

internal sealed class DbTeamMapping
{
    public void Map(EntityTypeBuilder<DbTeam> builder, Guid userId, Guid memberId, Guid teamId)
    {
        builder.ToTable("Teams");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.CreatedDate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ModifiedDate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ModifiedBy).IsRequired().HasMaxLength(50);
        var team = new DbTeam
        {
            Id = teamId,
            CreatedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            ModifiedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            CreatedBy = userId,
            ModifiedBy = userId,
            Title = "AirSoft Events"
        };
        builder.HasData(team);
    }
}
