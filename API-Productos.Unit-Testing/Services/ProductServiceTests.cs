using System;
using API_Productos.DTOs.Product;
using API_Productos.Interfaces;
using API_Productos.Models;
using API_Productos.Services;
using FluentAssertions;
using Moq;

namespace API_Productos.Unit_Testing.Services;

public class ProductServiceTests
{
    private readonly ProductService _productService;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productService = new ProductService(_productRepositoryMock.Object);
    }
    [Fact]
    public async Task GetByIdAsync_Returns_Product_When_Exists()
    {
        //arrange
        var product = new Product
        {
            Id = 1,
            Name = "Mouse",
            Description = "Mouse inalámbrico",
            Price = 25,
            Stock = 10
        };
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(product);
        //act
        var result = await _productService.GetByIdAsync(1);
        //asert   
        result.Should().NotBeNull();
        result!.Id.Should().Be(product.Id);
        result!.Name.Should().Be("Mouse");
        result!.Description.Should().Be("Mouse inalámbrico");
        result!.Price.Should().Be(25);
        result!.Stock.Should().Be(10);
    }

    [Fact]
    public async Task GetByIdAsync_Returns_Null_When_NotFound()
    {
        //arrange
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync((Product)null);
        //act
        var result = await _productService.GetByIdAsync(1);
        //assert
        result.Should().BeNull();
    }
    [Fact]
    public async Task CreateAsync_Should_Call_AddAsync()
    {
        //arrange
        var product = new ProductCreateDTO
        {
            Name = "Mouse",
            Description = "Mouse inalámbrico",
            Price = 25,
            Stock = 10
        };
        //act
        await _productService.CreateAsync(product);
        //assert
        _productRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Product>(product =>
            product.Name == "Mouse" &&
            product.Description == "Mouse inalámbrico" &&
            product.Price == 25 &&
            product.Stock == 10
        )), Times.Once);
    }
    [Fact]
    public async Task UpdateAsync_Should_Return_False_If_Not_Found()
    {
        //arrange
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Product?)null);
        //act
        var result = await _productService.UpdateAsync(new ProductUpdateDTO
        {
            Id = 99,
        });
        //assert
        result.Should().BeFalse();
        _productRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }
    [Fact]
    public async Task DeleteAsync_Should_Delete_When_Exists()
    {
        //arrange
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(new Product { Id = 1 });
        //act
        var result = await _productService.DeleteAsync(1);
        //assert
        result.Should().BeTrue();
        _productRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Product>()), Times.Once);
    }
    [Fact]
    public async Task DeleteAsync_Should_Return_False_When_Not_Found()
    {
        //arange
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(99))
            .ReturnsAsync((Product?)null);
        //act
        var result = await _productService.DeleteAsync(99);
        //asert
        result.Should().BeFalse();
        _productRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Product>()), Times.Never);
    }
}
