using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess.Entities;

namespace WebShop.DataAccess;

public class WebShopDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public WebShopDbContext(DbContextOptions options) : base(options) { }
}

//public DbSet<User> Users { get; set; }
//public DbSet<Order> Orders { get; set; }

//protected override void OnModelCreating(ModelBuilder modelBuilder)
//{
//    modelBuilder.Entity<OrderProduct>()
//        .HasKey(op => new { op.OrderId, op.ProductId });
//}
