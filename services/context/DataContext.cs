using Microsoft.EntityFrameworkCore;
using Services.Models;

namespace Services.Context;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    public DbSet<Trash> Trashs => Set<Trash>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Trash>().HasKey(p => p.TrashNID);
        // tambahan konfigurasi fluent API kalau perlu
    }
}
