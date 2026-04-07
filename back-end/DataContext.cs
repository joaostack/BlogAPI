using back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace back_end;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Name)
            .IsUnique();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
}
