using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirSoft.Data.Entity;

public class DbUserNavigation: DbEntity<int>
{
    public DbUserNavigation()
    {
    }
    private DbUserNavigation(ILazyLoader lazyLoader)
    {
        LazyLoader = lazyLoader;
    }

    private ILazyLoader LazyLoader { get; } = null!;

    private List<DbNavigationItem>? _navigationItems;

    public string Title { get; set; }

    public int RoleId { get; set; }

    public virtual DbUserRole? Role { get; set; }

    public virtual List<DbNavigationItem>? NavigationItems
    {
        get => LazyLoader.Load(this, ref _navigationItems);
        set => _navigationItems = value;
    }

    public virtual List<DbNavigationsToNavigationItems>? NavigationsToNavigationItems { get; set; }
}

internal sealed class DbUserNavigationMapping
{
    public void Map(EntityTypeBuilder<DbUserNavigation> builder)
    {
        builder.ToTable("UserNavigations");

        builder.HasKey(x => new { x.Id });
        builder.HasOne(x => x.Role).WithOne(x => x.UserNavigation).HasForeignKey<DbUserNavigation>(x => x.RoleId);

        builder.HasData(new List<DbUserNavigation>()
        {
            new DbUserNavigation()
            {
                Id = (int) UserRoleType.TeamLeader,
                Title = "Навигация Командира",
                ModifiedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                RoleId = (int) UserRoleType.TeamLeader
            },
            new DbUserNavigation()
            {
                Id = (int) UserRoleType.Player,
                Title = "Навигация Игрока",
                ModifiedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                RoleId = (int) UserRoleType.Player
            }
        });
    }
}