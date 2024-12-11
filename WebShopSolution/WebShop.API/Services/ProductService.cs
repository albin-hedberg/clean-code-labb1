using WebShop.API.Interfaces;
using WebShop.DataAccess.Entities;
using WebShop.DataAccess.Interfaces;

namespace WebShop.API.Services;

public class ProductService(IUnitOfWork unitOfWork) : IProductService
{
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await unitOfWork.ProductRepository.GetAllAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await unitOfWork.ProductRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Product product)
    {
        await unitOfWork.ProductRepository.AddAsync(product);
        await unitOfWork.CommitAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        await unitOfWork.ProductRepository.UpdateAsync(product);
        await unitOfWork.CommitAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ProductRepository.DeleteAsync(id);
        await unitOfWork.CommitAsync();
    }
}
