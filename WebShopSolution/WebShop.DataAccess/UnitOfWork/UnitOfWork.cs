using WebShop.DataAccess.Interfaces;
using WebShop.DataAccess.Repositories;

namespace WebShop.DataAccess.UnitOfWork;

public class UnitOfWork(WebShopDbContext context, IProductRepository productRepository) : IUnitOfWork
{
    public IProductRepository ProductRepository
    {
        get
        {
            return productRepository ??= new ProductRepository(context);
        }
    }

    public async Task<int> CommitAsync()
    {
        return await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
