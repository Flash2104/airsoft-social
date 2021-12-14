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

    public virtual List<DbUserRole>? Roles { get; set; }
    public virtual List<DbUserRolesToNavigationItems>? UserRolesToNavigationItems { get; set; }
}

internal sealed class DbNavigationItemsMapping
{
    public void Map(EntityTypeBuilder<DbNavigationItem> builder, Dictionary<int, Guid> roleNavIds)
    {
        builder.ToTable("NavigationItems");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Path).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Order).IsRequired().HasMaxLength(55);
        var items = BuildItems(builder, roleNavIds);


        // Bond Navigation to NavItems

        var joinedNavToNavItems = items.LeaderIds.Select(i => new DbNavigationsToNavigationItems()
        {
            NavigationId = roleNavIds[(int)UserRoleType.TeamManager],
            NavigationItemId = i
        }).Concat(
            items.PlayerIds.Select(y => new DbNavigationsToNavigationItems()
            {
                NavigationId = roleNavIds[(int)UserRoleType.Player],
                NavigationItemId = y
            })).ToList();

        builder
            .HasMany(x => x.Navigations)
            .WithMany(x => x.NavigationItems)
            .UsingEntity<DbNavigationsToNavigationItems>(x => x.
                HasData(
                    joinedNavToNavItems
                    ));


        // Bond User Roles to NavItems

        var joinedRolesToNavItems = items.LeaderIds.Select(i => new DbUserRolesToNavigationItems()
        {
            RoleId = (int)UserRoleType.TeamManager,
            NavigationItemId = i
        }).Concat(
            items.PlayerIds.Select(y => new DbUserRolesToNavigationItems()
            {
                RoleId = (int)UserRoleType.Player,
                NavigationItemId = y
            })).ToList();
        builder
            .HasMany(x => x.Roles)
            .WithMany(x => x.NavigationItems)
            .UsingEntity<DbUserRolesToNavigationItems>(x => x.
                HasData(
                    joinedRolesToNavItems
                ));
    }

    private NavItems BuildItems(EntityTypeBuilder<DbNavigationItem> builder, Dictionary<int, Guid> roleNavIds)
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
            new DbNavigationItem() // Команда/Игроки
            {
                Id = 3,
                Title = "Игроки",
                Icon = "people",
                Path = "/private/team/players",
                ParentId = 2,
                Order = 1
            },
            new DbNavigationItem() // Команда/Редактирование
            {
                Id = 4,
                Title = "Редактировать",
                Icon = "edit",
                Path = "/private/team/edit",
                ParentId = 2,
                Order = 2,
            },
            new DbNavigationItem() // Команда/Заявки
            {
                Id = 5,
                Title = "Заявки",
                Icon = "group_add",
                Path = "/private/team/requests",
                ParentId = 4,
                Order = 3,
            },
            new DbNavigationItem()
            {
                Id = 6,
                Title = "События",
                Icon = "calendar_view_month",
                Path = "/private/events",
                Disabled = true,
                Order = 3
            },

        };
        builder.HasData(items);
        var res = new NavItems()
        {
            PlayerIds = new List<int>() { 1, 2, 3, 6 },
            LeaderIds = new List<int>() { 1, 2, 3, 4, 5, 6 },
        };
        return res;
    }

    class NavItems
    {
        public List<int> PlayerIds { get; init; } = null!;
        public List<int> LeaderIds { get; init; } = null!;
    }
}