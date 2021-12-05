
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirSoft.Data.Entity;

public class DbMember : DbEntity<Guid>
{
    public DbMember()
    {
    }

    private DbMember(ILazyLoader lazyLoader)
    {
        LazyLoader = lazyLoader;
    }

    private ILazyLoader LazyLoader { get; set; } = null!;

    private List<DbMemberRole>? _memberRoles;
    private DbTeam? _team;
    private DbUser? _user;

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public byte[] Avatar { get; set; } = null!;

    public Guid? TeamId { get; set; }

    public DbTeam? Team
    {
        get => LazyLoader.Load(this, ref _team);
        set => _team = value;
    }

    public Guid? UserId { get; set; }

    public DbUser? User
    {
        get => LazyLoader.Load(this, ref _user);
        set => _user = value;
    }

    public List<DbMemberRole>? MemberRoles
    {
        get => LazyLoader.Load(this, ref _memberRoles);
        set => _memberRoles = value;
    }

    public virtual List<DbMembersToRoles>? MembersToRoles { get; set; }
}

internal sealed class DbMemberMapping
{
    public void Map(EntityTypeBuilder<DbMember> builder, Guid userId, Guid memberId, Guid teamId)
    {
        builder.ToTable("Members");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Surname).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Avatar);

        builder.Property(x => x.CreatedDate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ModifiedDate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ModifiedBy).IsRequired().HasMaxLength(50);

        builder.HasOne(x => x.Team).WithMany(x => x.Members).HasForeignKey(x => x.TeamId);
        builder.HasOne(x => x.User).WithOne(x => x.Member).HasForeignKey<DbMember>(x => x.UserId);
        string root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); ;
        var adminProfile = new DbMember()
        {
            Id = memberId,
            CreatedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            ModifiedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            CreatedBy = userId,
            ModifiedBy = userId,
            Name = "Кирилл",
            Surname = "Хоруженко",
            TeamId = teamId,
            UserId = userId,
            Avatar = File.ReadAllBytes(root + "\\InitialData\\photo.jpg")
        };
        builder.HasData(adminProfile);
        builder.HasMany(x => x.MemberRoles).WithMany(x => x.Members).UsingEntity<DbMembersToRoles>(
            x => x.HasData(new DbMembersToRoles()
            {
                MemberId = memberId,
                RoleId = (int)MemberRoleType.Leader
            })
            );
    }
}