using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess.Entities;

namespace WebShop.DataAccess;

public class WebShopDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public WebShopDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Test", Description = "Product", Price = 10.0 },
            new Product { Id = 2, Name = "2Test", Description = "2Product", Price = 20.0 },
            new Product { Id = 3, Name = "Test 3", Description = "Product Drift", Price = 30.0 }
        );
    }
}
