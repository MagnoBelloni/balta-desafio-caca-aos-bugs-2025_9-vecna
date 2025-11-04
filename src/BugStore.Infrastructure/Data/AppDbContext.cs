using BugStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderLine> OrderLines { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Name);

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Email);

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Phone);

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Title);
        
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Description);

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Slug);

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Price);

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.CreatedAt);

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.TotalAmount);
    }
}