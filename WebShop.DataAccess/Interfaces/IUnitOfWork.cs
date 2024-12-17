namespace WebShop.DataAccess.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IProductRepository ProductRepository { get; }
    Task<int> CommitAsync();
}
