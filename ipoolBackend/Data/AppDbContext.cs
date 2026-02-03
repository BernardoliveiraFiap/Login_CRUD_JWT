using Microsoft.EntityFrameworkCore;
using ipoolBackend.Models;

namespace ipoolBackend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.NormalizedEmail)
            .HasMaxLength(150)
            .IsRequired();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.NormalizedEmail)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}
