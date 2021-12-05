
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirSoft.Data.Entity;

public class DbMembersToRoles
{
    public Guid MemberId { get; set; }
    public virtual DbMember? Member { get; set; }

    public int RoleId { get; set; }
    public virtual DbMemberRole? Role { get; set; }
}

internal sealed class DbMembersToRolesMapping
{
    public void Map(EntityTypeBuilder<DbMembersToRoles> builder)
    {
        builder.ToTable("MembersToRoles");

        builder.HasKey(x => new {ProfileId = x.MemberId, x.RoleId });
        builder
             .HasOne(x => x.Member)
             .WithMany(x => x.MembersToRoles)
             .HasForeignKey(j => j.MemberId);
        builder
            .HasOne(x => x.Role)
            .WithMany(x => x.MembersToRoles)
            .HasForeignKey(j => j.RoleId);
    }
}
