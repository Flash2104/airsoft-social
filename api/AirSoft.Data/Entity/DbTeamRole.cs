using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirSoft.Data.Entity;

public class DbTeamRole : DbEntity<Guid>
{
    public DbTeamRole()
    {
    }
    private DbTeamRole(ILazyLoader lazyLoader)
    {
        LazyLoader = lazyLoader;
    }

    private ILazyLoader LazyLoader { get; set; } = null!;

    private DbMemberRole? _role;

    public int RoleId { get; set; }

    public DbMemberRole? Role
    {
        get => LazyLoader.Load(this, ref _role);
        set => _role = value;
    }

    public int Rank { get; set; }

    public Guid? TeamId { get; set; }

    public virtual DbTeam? Team { get; set; }

    public virtual List<DbTeamRolesToMembers>? TeamRolesToMembers { get; set; }

    public virtual List<DbMember>? TeamMembers { get; set; }
}


internal sealed class DbTeamRolesMapping
{
    public void Map(EntityTypeBuilder<DbTeamRole> builder, Guid teamId, Dictionary<int, Guid> ids)
    {
        builder.ToTable("TeamRoles");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Rank).IsRequired().HasMaxLength(255);
        builder.HasOne(x => x.Role).WithMany(x => x.TeamRoles).HasForeignKey(c => c.RoleId);
        builder.HasOne(x => x.Team).WithMany(x => x.TeamRoles).HasForeignKey(c => c.TeamId);

        var roles = Enum.GetValues<DefaultMemberRoleType>().Select(v => new DbTeamRole
        {
            Id = ids[(int)v],
            RoleId = (int)v,
            CreatedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            ModifiedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            Rank = (int)v,
            TeamId = teamId
        })
            .ToArray();
        builder.HasData(roles);
    }
}