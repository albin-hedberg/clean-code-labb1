using Microsoft.AspNetCore.Mvc;
using Moq;
using WebShop.API.Controllers;
using WebShop.API.Interfaces;
using WebShop.DataAccess.Entities;

namespace WebShop.Tests.Controllers;

public class ProductControllerTests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _mockProductService = new Mock<IProductService>();
        _controller = new ProductController(_mockProductService.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOkResult_WithListOfProducts()
    {
        // Arrange
        var products = new List<Product> { new Product(), new Product() };
        _mockProductService.Setup(service => service.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _controller.GetAllAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<Product>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOkResult_WithProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Name", Description = "Description", Price = 0};
        _mockProductService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _controller.GetByIdAsync(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<Product>(okResult.Value);
        Assert.Equal(product, returnValue);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNotFound_WhenProductIsNull()
    {
        // Arrange
        _mockProductService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync((Product)null);

        // Act
        var result = await _controller.GetByIdAsync(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task AddAsync_ReturnsCreatedResult_WithProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Name", Description = "Description", Price = 0 };
        _mockProductService.Setup(service => service.AddAsync(product)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddAsync(product);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        var returnValue = Assert.IsType<Product>(createdResult.Value);
        Assert.Equal(product, returnValue);
    }

    [Fact]
    public async Task AddAsync_ReturnsBadRequest_WhenProductIsNull()
    {
        // Act
        var result = await _controller.AddAsync(null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Product is null.", badRequestResult.Value);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsOkResult()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Name", Description = "Description", Price = 0 };
        _mockProductService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(product);
        _mockProductService.Setup(service => service.UpdateAsync(product)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateAsync(product);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Name", Description = "Description", Price = 0};
        _mockProductService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync((Product)null);

        // Act
        var result = await _controller.UpdateAsync(product);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Theory]
    [InlineData(null, "Description", 10)]
    [InlineData("Name", null, 10)]
    [InlineData("Name", "Description", -10)]
    public async Task UpdateAsync_ReturnsBadRequest_ForInvalidProductFields(string name, string description, double price)
    {
        // Arrange
        var product = new Product { Id = 1, Name = name, Description = description, Price = price };

        // Act
        var result = await _controller.UpdateAsync(product);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("All product fields must be provided.", badRequestResult.Value);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsOkResult()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Name", Description = "Description", Price = 0 };
        _mockProductService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(product);
        _mockProductService.Setup(service => service.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteAsync(1);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        _mockProductService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync((Product)null);

        // Act
        var result = await _controller.DeleteAsync(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
