using Microsoft.EntityFrameworkCore;
using Moq;
using WebShop.DataAccess;
using WebShop.DataAccess.Entities;
using WebShop.DataAccess.Interfaces;

namespace WebShop.Tests.UnitOfWork;

public class UnitOfWorkTests
{
    private readonly WebShopDbContext _context;
    private readonly DataAccess.UnitOfWork.UnitOfWork _unitOfWork;
    private readonly Mock<IProductRepository> _productRepositoryMock;

    public UnitOfWorkTests()
    {
        var options = new DbContextOptionsBuilder<WebShopDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new WebShopDbContext(options);
        _productRepositoryMock = new Mock<IProductRepository>();
        _unitOfWork = new DataAccess.UnitOfWork.UnitOfWork(_context, _productRepositoryMock.Object);
    }

    [Fact]
    public void ProductRepository_ShouldReturnProductRepositoryInstance()
    {
        // Act
        var productRepository = _unitOfWork.ProductRepository;

        // Assert
        Assert.NotNull(productRepository);
    }

    [Fact]
    public async Task CommitAsync_ShouldSaveChanges()
    {
        // Arrange
        _context.Products.Add(new Product { Id = 1, Name = "Test Product", Description = "Test Description", Price = 123.45 });

        // Act
        var result = await _unitOfWork.CommitAsync();

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void Dispose_ShouldDisposeContext()
    {
        // Act
        _unitOfWork.Dispose();

        // Assert
        Assert.Throws<ObjectDisposedException>(() => _context.Products.Add(new Product { Id = 2, Name = "Another Product" }));
    }
}
