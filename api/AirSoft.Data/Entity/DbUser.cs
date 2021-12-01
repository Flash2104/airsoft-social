
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirSoft.Data.Entity;

public class DbUser
{
    public Guid Id { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid ModifiedBy { get; set; }

    public DateTime AddedDate { get; set; }

    public DateTime ModifiedDate { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? PasswordHash { get; set; }

    public string HashPassword(string password)
    {
        var ph = new PasswordHasher<DbUser>();
        return ph.HashPassword(this, password);
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
        var adminID = Guid.Parse("fadde9ec-7dc4-4033-b1e6-2f83a08c70f3");
        builder.HasData(new DbUser
        {
            Id = adminID,
            AddedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            ModifiedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            CreatedBy = adminID,
            ModifiedBy = adminID,
            Email = "khoruzhenko.work@gmail.com",
            PasswordHash = "AQAAAAEAACcQAAAAEMQnvSxDqgyc+KNNzIFjcuST/qZGfHVSLT9P+Z3revJP2Q9Tctz8PIeDxj2k7aJkLg==",
            Phone = "89266762453"
        });
    }
}
