using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirSoft.Data.Entity;

public class DbUserRole
{
    public int Id { get; set; }

    public string Role { get; set; } = null!;

    public virtual List<DbUser>? Users { get; set; }
    public virtual List<DbUsersToRoles>? UsersToRoles { get; set; }
}

public enum UserRoleType
{
    Creator = 1,
    Administrator = 2,
    //Organizer = 3,
    //TeamLeader = 4,
    //Sponsor = 5,
    //Merchant = 6,
    //Private = 7,
    User = 3
}

internal sealed class DbUserRolesMapping
{
    public void Map(EntityTypeBuilder<DbUserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Role).IsRequired().HasMaxLength(255);
        builder.HasMany(x => x.Users).WithMany(x => x.UserRoles).UsingEntity<DbUsersToRoles>();

        var roles = Enum.GetValues<UserRoleType>().Select(v => new DbUserRole { Id = (int)v, Role = v.ToString() })
            .ToArray();
        builder.HasData(roles);
    }
}