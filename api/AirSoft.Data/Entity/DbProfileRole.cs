using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirSoft.Data.Entity;

public class DbProfileRole
{
    public int Id { get; set; }

    public string Role { get; set; } = null!;

    public virtual List<DbProfile>? Profiles { get; set; }
    public virtual List<DbProfilesToRoles>? ProfilesToRoles { get; set; }
}

public enum ProfileRoleType
{
    None = 0,
    Organizer = 1,
    TeamLeader = 2,
    Sponsor = 3,
    Merchant = 4,
    Deputy = 5,
    Private = 6
}

internal sealed class DbProfileRolesMapping
{
    public void Map(EntityTypeBuilder<DbProfileRole> builder)
    {
        builder.ToTable("ProfileRoles");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Role).IsRequired().HasMaxLength(255);
        builder.HasMany(x => x.Users).WithMany(x => x.UserRoles).UsingEntity<DbUsersToRoles>();

        var roles = Enum.GetValues<DbProfileRole>().Select(v => new DbUserRole { Id = (int)v, Role = v.ToString() })
            .ToArray();
        builder.HasData(roles);
    }
}