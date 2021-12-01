using AirSoft.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace AirSoft.Data;

public interface IDbContext : IDisposable
{
    DbSet<DbUser>? Users { get; set; }

    Task SaveAsync();

    public void Initialize();
}

public class AirSoftDbContext : DbContext, IDbContext
{
    public AirSoftDbContext()
    {

    }

    public AirSoftDbContext(DbContextOptions<AirSoftDbContext> options) : base(options)
    {
    }

    public DbSet<DbUser>? Users { get; set; }

    public async Task SaveAsync()
    {
        await base.SaveChangesAsync();
    }

    public void Initialize()
    {
        try
        {
            this.Database.Migrate();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Data context initialization failed.", ex);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dbo");

        new DbUserMapping().Map(modelBuilder.Entity<DbUser>());
    }
}
