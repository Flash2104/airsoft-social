
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirSoft.Data.Entity;

public class DbTeam: DbEntity<Guid>
{
    public DbTeam()
    {
    }
    
    public string Title { get; set; } = null!;

    public List<DbProfile>? Members { get; set; }

    public DbProfile? Leader { get; set; }

    public ICollection<DbUserRole>? UserRoles
    {
        get => LazyLoader.Load(this, ref _userRoles);
        set => _userRoles = value;
    }

    public virtual List<DbUsersToRoles>? UsersToRoles { get; set; }

    public string HashPassword(string password)
    {
        var ph = new PasswordHasher<DbUser>();
        return ph.HashPassword(this, password);
    }

    public bool ValidPassword(string password)
    {
        var ph = new PasswordHasher<DbUser>();
        var result = ph.VerifyHashedPassword(this, PasswordHash, password);
        return result == PasswordVerificationResult.Success;
    }
}

internal sealed class DbUserMapping
{
    public void Map(EntityTypeBuilder<DbUser> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(255);
        builder.Property(x => x.AddedDate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ModifiedDate).IsRequired().HasMaxLength(50);
        var adminId = Guid.Parse("fadde9ec-7dc4-4033-b1e6-2f83a08c70f3");

        var admin = new DbUser
        {
            Id = adminId,
            AddedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            ModifiedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            CreatedBy = adminId,
            ModifiedBy = adminId,
            Email = "khoruzhenko.work@gmail.com",
            PasswordHash = "AQAAAAEAACcQAAAAEMQnvSxDqgyc+KNNzIFjcuST/qZGfHVSLT9P+Z3revJP2Q9Tctz8PIeDxj2k7aJkLg==",
            Phone = "89266762453"
        };
        builder.HasData(admin);
        builder.HasMany(x => x.UserRoles).WithMany(x => x.Users).UsingEntity<DbUsersToRoles>(
            x => x.HasData(new DbUsersToRoles()
            {
                UserId = adminId,
                RoleId = (int)UserRoleType.Creator
            })
            );
    }
}
