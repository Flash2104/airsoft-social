using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirSoft.Data.Entity;

public class DbMemberRole : DbEntity<int>
{
    public string Title { get; set; } = null!;

    public virtual List<DbTeamRole>? TeamRoles { get; set; }
}

public enum DefaultMemberRoleType
{
    Командир = 1,
    Заместитель = 2,
    Рядовой = 3
}

internal sealed class DbMemberRolesMapping
{
    public void Map(EntityTypeBuilder<DbMemberRole> builder)
    {
        builder.ToTable("MemberRoles");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Title).IsRequired().HasMaxLength(255);

        var roles = Enum.GetValues<DefaultMemberRoleType>().Select(v => new DbMemberRole
        {
            Id = (int)v,
            Title = v.ToString(),
            CreatedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            ModifiedDate = new DateTime(2021, 12, 02, 1, 50, 00),
        })
            .ToArray();
        builder.HasData(roles);
    }
}