using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess.Entities;
using WebShop.DataAccess.Interfaces;

namespace WebShop.DataAccess.Repositories;

public class ProductRepository(WebShopDbContext context) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await context.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task AddAsync(Product product)
    {
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        context.Products.Remove(product);
        await context.SaveChangesAsync();
    }
}
