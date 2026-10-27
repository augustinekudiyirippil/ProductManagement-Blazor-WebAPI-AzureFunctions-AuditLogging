using FluentAssertions;
using Moq;
using ProductManagement.Api.DTOs;
using ProductManagement.Api.Models;
using ProductManagement.Api.Repositories;
using ProductManagement.Api.Services;

namespace ProductManagement.Api.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<IAuditFunctionService> _auditMock;

    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _repositoryMock =
            new Mock<IProductRepository>();

        _auditMock =
            new Mock<IAuditFunctionService>();

        _service =
            new ProductService(
                _repositoryMock.Object,
                _auditMock.Object);
    }



    [Fact]
    public async Task GetAllProductsAsync_ShouldReturnProducts()
    {
        // Arrange

        var products = new List<Product>
    {
        new()
        {
            Id = 1,
            Name = "Dell"
        }
    };

        _repositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(products);

        // Act

        var result =
            await _service.GetAllProductsAsync();

        // Assert

        result.Should()
            .HaveCount(1);

        result.First().Name
            .Should()
            .Be("Dell");
    }




    [Fact]
    public async Task CreateProductAsync_ShouldSaveProduct()
    {
        // Arrange

        var product = new Product
        {
            Name = "Dell",
            Price = 1000
        };

        _repositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Product>()))
            .ReturnsAsync(product);

        // Act

        var result =
            await _service.CreateProductAsync(
                product,
                5);

        // Assert

        result.Should().NotBeNull();

        _repositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Product>()),
            Times.Once);
    }



    [Fact]
    public async Task CreateProductAsync_ShouldCallAuditFunction()
    {
        // Arrange

        var product = new Product
        {
            Id = 1,
            Name = "Dell"
        };

        _repositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Product>()))
            .ReturnsAsync(product);

        // Act

        await _service.CreateProductAsync(
            product,
            5);

        // Assert

        _auditMock.Verify(
            x => x.LogAsync(
                It.IsAny<ProductAuditRequest>()),
            Times.Once);
    }



    [Fact]
    public async Task UpdateProductAsync_ShouldUpdateProduct()
    {
        // Arrange

        var product = new Product
        {
            Id = 1,
            Name = "Dell"
        };

        // Act

        await _service.UpdateProductAsync(
            product,
            5);

        // Assert

        _repositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<Product>()),
            Times.Once);

        _auditMock.Verify(
            x => x.LogAsync(
                It.IsAny<ProductAuditRequest>()),
            Times.Once);
    }




    [Fact]
    public async Task DeleteProductAsync_ShouldDeleteProduct()
    {
        // Arrange

        var product = new Product
        {
            Id = 1,
            Name = "Dell"
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(product);

        // Act

        await _service.DeleteProductAsync(
            1,
            5,
            "Dell");

        // Assert

        _repositoryMock.Verify(
            x => x.DeleteAsync(product),
            Times.Once);

        _auditMock.Verify(
            x => x.LogAsync(
                It.IsAny<ProductAuditRequest>()),
            Times.Once);
    }


    [Fact]
    public async Task MakeUnavailableAsync_ShouldDisableProduct()
    {
        // Arrange

        var product = new Product
        {
            Id = 1,
            IsAvailable = true
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(product);

        // Act

        var result =
            await _service.MakeUnavailableAsync(
                1,
                5,
                "Dell");

        // Assert

        result.Should().BeTrue();

        product.IsAvailable
            .Should().BeFalse();
    }








    [Fact]
    public async Task MakeAvailableAsync_ShouldEnableProduct()
    {
        // Arrange

        var product = new Product
        {
            Id = 1,
            IsAvailable = false
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(product);

        // Act

        var result =
            await _service.MakeAvailableAsync(
                1,
                5,
                "Dell");

        // Assert

        result.Should().BeTrue();

        product.IsAvailable
            .Should().BeTrue();
    }












}
