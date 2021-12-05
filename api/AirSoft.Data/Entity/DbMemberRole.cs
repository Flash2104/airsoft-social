using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirSoft.Data.Entity;

public class DbMemberRole
{
    public int Id { get; set; }

    public string Role { get; set; } = null!;

    public virtual List<DbMember>? Members { get; set; }
    public virtual List<DbMembersToRoles>? MembersToRoles { get; set; }
}

public enum MemberRoleType
{
    None = 0,
    Leader = 1,
    Deputy = 2,
    Private = 3
}

internal sealed class DbMemberRolesMapping
{
    public void Map(EntityTypeBuilder<DbMemberRole> builder)
    {
        builder.ToTable("MemberRoles");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Role).IsRequired().HasMaxLength(255);
        builder.HasMany(x => x.Members).WithMany(x => x.MemberRoles).UsingEntity<DbMembersToRoles>();

        var roles = Enum.GetValues<MemberRoleType>().Where(x => x != MemberRoleType.None).Select(v => new DbMemberRole { Id = (int)v, Role = v.ToString() })
            .ToArray();
        builder.HasData(roles);
    }
}