using Moq;
using WebShop.API.Interfaces;
using WebShop.API.Services;
using WebShop.DataAccess.Entities;
using WebShop.DataAccess.Interfaces;

namespace WebShop.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly IProductService _productService;

    public ProductServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _unitOfWorkMock.Setup(u => u.ProductRepository).Returns(_productRepositoryMock.Object);
        _productService = new ProductService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var products = new List<Product> { new Product(), new Product() };
        _productRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _productService.GetAllAsync();

        // Assert
        Assert.Equal(products, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var product = new Product { Id = 1 };
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _productService.GetByIdAsync(1);

        // Assert
        Assert.Equal(product, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Product)null);

        // Act
        var result = await _productService.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldAddProduct()
    {
        // Arrange
        var product = new Product();

        // Act
        await _productService.AddAsync(product);

        // Assert
        _productRepositoryMock.Verify(repo => repo.AddAsync(product), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProduct()
    {
        // Arrange
        var product = new Product();

        // Act
        await _productService.UpdateAsync(product);

        // Assert
        _productRepositoryMock.Verify(repo => repo.UpdateAsync(product), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteProduct()
    {
        // Arrange
        var productId = 1;

        // Act
        await _productService.DeleteAsync(productId);

        // Assert
        _productRepositoryMock.Verify(repo => repo.DeleteAsync(productId), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }
}
