
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirSoft.Data.Entity;

public class DbProfilesToRoles
{
    public Guid ProfileId { get; set; }
    public virtual DbProfile? Profile { get; set; }

    public int RoleId { get; set; }
    public virtual DbProfileRole? Role { get; set; }
}

internal sealed class DbProfilesToRolesMapping
{
    public void Map(EntityTypeBuilder<DbProfilesToRoles> builder)
    {
        builder.ToTable("UsersToRoles");

        builder.HasKey(x => new { x.ProfileId, x.RoleId });
        builder
             .HasOne(x => x.Role)
             .WithMany(x => x.UsersToRoles)
             .HasForeignKey(j => j.RoleId);
        builder
            .HasOne(x => x.Profile)
            .WithMany(x => x.UsersToRoles)
            .HasForeignKey(j => j.UserId);
    }
}
