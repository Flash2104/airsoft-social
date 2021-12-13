using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirSoft.Data.Entity;

public class DbNavigationItem : DbEntity<int>
{
    public string Path { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Icon { get; set; }
    public int? ParentId { get; set; }
    public bool? Disabled { get; set; }
    public int Order { get; set; }

    public virtual List<DbUserNavigation>? Navigations { get; set; }
    public virtual List<DbNavigationsToNavigationItems>? NavigationsToNavigationItems { get; set; }
}

internal sealed class DbNavigationItemsMapping
{
    public void Map(EntityTypeBuilder<DbNavigationItem> builder)
    {
        builder.ToTable("NavigationItems");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Path).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Order).IsRequired().HasMaxLength(55);
        var items = BuildItems(builder);
        var joined = items.LeaderIds.Select(i => new DbNavigationsToNavigationItems()
        {
            NavigationId = (int)UserRoleType.TeamLeader,
            NavigationItemId = i
        }).Concat(
            items.PlayerIds.Select(y => new DbNavigationsToNavigationItems()
            {
                NavigationId = (int)UserRoleType.Player,
                NavigationItemId = y
            })).ToList();

        builder
            .HasMany(x => x.Navigations)
            .WithMany(x => x.NavigationItems)
            .UsingEntity<DbNavigationsToNavigationItems>(x => x.
                HasData(
                    joined
                    ));
    }

    private NavItems BuildItems(EntityTypeBuilder<DbNavigationItem> builder)
    {
        var items = new List<DbNavigationItem>()
        {
            new DbNavigationItem()
            {
                Id = 1,
                Title = "Профиль",
                Icon = "person",
                Path = "/private/profile",
                Order = 1
            },
            new DbNavigationItem()
            {
                Id = 2,
                Title = "Команда",
                Icon = "groups",
                Path = "/private/team",
                Order = 2
            },
            new DbNavigationItem()
            {
                Id = 3,
                Title = "События",
                Icon = "calendar_view_month",
                Path = "/private/events",
                Disabled = true,
                Order = 3
            },
            new DbNavigationItem()
            {
                Id = 4,
                Title = "Заявки",
                Icon = "check",
                Path = "/private/team/requests",
                ParentId = 2,
                Order = 2
            },
            new DbNavigationItem()
            {
                Id = 5,
                Title = "Редактировать",
                Icon = "edit",
                Path = "/private/team/edit",
                ParentId = 2,
                Order = 1
            },
        };
        builder.HasData(items);
        var res = new NavItems()
        {
            PlayerIds = new List<int>() { 1, 2, 3 },
            LeaderIds = new List<int>() { 1, 2, 3, 4, 5 },
        };
        return res;
    }

    class NavItems
    {
        public List<int> PlayerIds { get; init; } = null!;
        public List<int> LeaderIds { get; init; } = null!;
    }
}