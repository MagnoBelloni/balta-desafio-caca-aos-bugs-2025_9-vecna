using BugStore.Domain.Entities;
using BugStore.Infrastructure.Data;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("=== SQLite → Azure SQL Transfer ===");

var batchSize = 10000;

//The app is running inside bin/Debug/net10.0, so we need to go back to the project root
var sqlitepath = Path.GetFullPath("./../../../app.db");
Console.WriteLine($"File exists? {File.Exists(sqlitepath)}");
var sqliteOptions = new DbContextOptionsBuilder<AppDbContextSqlite>()
    .UseSqlite($"Data Source={sqlitepath}")
    .Options;

var azureOptions = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer("connection")
    .Options;

using var sqliteContext = new AppDbContextSqlite(sqliteOptions);
using var azureContext = new AppDbContext(azureOptions);

var total = await sqliteContext.Customers.CountAsync();
Console.WriteLine($"Found {total} Customers in SQLite.");

int processed = 0;
while (true)
{
    var batch = await sqliteContext.Customers
        .OrderBy(x => x.Id)
        .Skip(processed)
        .Take(batchSize)
        .ToListAsync();

    if (batch.Count == 0)
        break;

    await azureContext.BulkInsertOrUpdateAsync(batch);

    //azureContext.Products.AddRange(batch);
    //await azureContext.SaveChangesAsync();

    processed += batch.Count;
    Console.WriteLine($"Transferred {processed}/{total}");
}

Console.WriteLine("✅ Transfer complete!");


public class AppDbContextSqlite(DbContextOptions<AppDbContextSqlite> options) : DbContext(options)
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

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Title);

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.CreatedAt);

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.TotalAmount);
    }
}