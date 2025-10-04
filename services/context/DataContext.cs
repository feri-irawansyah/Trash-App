using Microsoft.EntityFrameworkCore;
using Services.Models;

namespace Services.Context;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    public DbSet<Trash> Trashs => Set<Trash>();
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserNID);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Role).IsRequired();
        });
        modelBuilder.Entity<Trash>().HasKey(p => p.TrashNID);
        // tambahan konfigurasi fluent API kalau perlu
    }
}
