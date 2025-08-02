using System;
using API_Productos.DTOs.Product;
using API_Productos.Integration_Testing.Helper;
using API_Productos.Repositories;
using API_Productos.Services;
using FluentAssertions;

namespace API_Productos.Integration_Testing.Services;

public class ProductServiceTests
{
    [Fact]
    public async Task Create_Then_GetById_Should_Return_Product()
    {
        // Arrange
        using var context = TestHelper.createInMemoryDbContext();
        var repo = new ProductRepository(context);
        var service = new ProductService(repo);
        var dto = new ProductCreateDTO
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 100.0m,
            Stock = 10
        };
        //act
        await service.CreateAsync(dto);
        //assert
        var product = await service.GetByIdAsync(1);
        product.Should().NotBeNull();
        product!.Name.Should().Be(dto.Name);
        product!.Description.Should().Be(dto.Description);
        product!.Price.Should().Be(dto.Price);
        product!.Stock.Should().Be(dto.Stock);
        product!.Id.Should().Be(1);
    }
    [Fact]
    public async Task Update_Should_Modify_Existing_Product()
    {
        // Arrange
        using var context = TestHelper.createInMemoryDbContext();
        var repo = new ProductRepository(context);
        var service = new ProductService(repo);
        var createDto = new ProductCreateDTO
        {
            Name = "Initial Product",
            Description = "Initial Description",
            Price = 50.0m,
            Stock = 5
        };
        await service.CreateAsync(createDto);
        var updateDto = new ProductUpdateDTO
        {
            Id = 1,
            Name = "Updated Product",
            Description = "Updated Description",
            Price = 75.0m,
            Stock = 10
        };
        // Act
        var result = await service.UpdateAsync(updateDto);
        // Assert
        result.Should().BeTrue();
        var updatedProduct = await service.GetByIdAsync(1);
        updatedProduct.Should().NotBeNull();
        updatedProduct!.Name.Should().Be(updateDto.Name);
        updatedProduct!.Description.Should().Be(updateDto.Description);
        updatedProduct!.Price.Should().Be(updateDto.Price);
        updatedProduct!.Stock.Should().Be(updateDto.Stock);
        updatedProduct!.Id.Should().Be(1);
    }
    [Fact]
    public async Task Delete_Should_Remove_Product()
    {
        // Arrange
        using var context = TestHelper.createInMemoryDbContext();
        var repo = new ProductRepository(context);
        var service = new ProductService(repo);
        var createDto = new ProductCreateDTO
        {
            Name = "Product to Delete",
            Description = "Description",
            Price = 20.0m,
            Stock = 2
        };
        await service.CreateAsync(createDto);
        // Act
        var deleteResult = await service.DeleteAsync(1);
        // Assert
        deleteResult.Should().BeTrue();
        var deletedProduct = await service.GetByIdAsync(1);
        deletedProduct.Should().BeNull();
    }
    [Fact]
    public async Task GetAll_Should_Filter_By_Name()
    {
        using var context = TestHelper.createInMemoryDbContext();
        var repo = new ProductRepository(context);
        var service = new ProductService(repo);

        await service.CreateAsync(new ProductCreateDTO { Name = "Laptop", Description = "HP", Price = 500, Stock = 2 });
        await service.CreateAsync(new ProductCreateDTO { Name = "Teclado", Description = "Mecánico", Price = 30, Stock = 5 });

        var result = await service.GetAllAsync("Laptop", null, null, null, 1, 10);

        result.Data.Should().HaveCount(1);
        result?.Data?[0].Name.Should().Be("Laptop");
    }

    [Fact]
    public async Task GetAll_Should_Filter_By_Description()
    {
        using var context = TestHelper.createInMemoryDbContext();
        var repo = new ProductRepository(context);
        var service = new ProductService(repo);

        await service.CreateAsync(new ProductCreateDTO { Name = "Mouse", Description = "Bluetooth", Price = 20, Stock = 3 });
        await service.CreateAsync(new ProductCreateDTO { Name = "Mouse", Description = "USB", Price = 15, Stock = 4 });

        var result = await service.GetAllAsync(null, "USB", null, null, 1, 10);

        result.Data.Should().HaveCount(1);
        result?.Data?[0].Description.Should().Be("USB");
    }

    [Fact]
    public async Task GetAll_Should_Filter_By_Price()
    {
        using var context = TestHelper.createInMemoryDbContext();
        var repo = new ProductRepository(context);
        var service = new ProductService(repo);

        await service.CreateAsync(new ProductCreateDTO { Name = "Monitor", Description = "24 pulgadas", Price = 150, Stock = 1 });
        await service.CreateAsync(new ProductCreateDTO { Name = "Monitor", Description = "27 pulgadas", Price = 200, Stock = 2 });

        var result = await service.GetAllAsync(null, null, 200, null, 1, 10);

        result.Data.Should().HaveCount(1);
        result?.Data?[0].Price.Should().Be(200);
    }

    [Fact]
    public async Task GetAll_Should_Filter_By_Stock()
    {
        using var context = TestHelper.createInMemoryDbContext();
        var repo = new ProductRepository(context);
        var service = new ProductService(repo);

        await service.CreateAsync(new ProductCreateDTO { Name = "Tablet", Description = "Android", Price = 300, Stock = 2 });
        await service.CreateAsync(new ProductCreateDTO { Name = "Tablet", Description = "iOS", Price = 600, Stock = 5 });

        var result = await service.GetAllAsync(null, null, null, 5, 1, 10);

        result.Data.Should().HaveCount(1);
        result?.Data?[0].Stock.Should().Be(5);
    }

    [Fact]
    public async Task GetAll_Should_Paginate_Results()
    {
        using var context = TestHelper.createInMemoryDbContext();
        var repo = new ProductRepository(context);
        var service = new ProductService(repo);

        for (int i = 1; i <= 25; i++)
        {
            await service.CreateAsync(new ProductCreateDTO
            {
                Name = $"Producto {i}",
                Description = $"Desc {i}",
                Price = i * 10,
                Stock = i
            });
        }

        var result = await service.GetAllAsync(null, null, null, null, page: 2, pageSize: 10);

        result.Page.Should().Be(2);
        result.PageSize.Should().Be(10);
        result.Data.Should().NotBeNull();
        result.Data.Should().HaveCount(10);
        result.TotalRecords.Should().Be(25);
        result.TotalPages.Should().Be(3);
    }

}
