using System;
using API_Productos.Controllers;
using API_Productos.DTOs.ApiResponse;
using API_Productos.DTOs.Product;
using API_Productos.Interfaces;
using API_Productos.Models;
using FluentAssertions;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API_Productos.Unit_Testing.Controllers;

public class ProductControllerTests
{
    private readonly Mock<IProductService> _serviceMock;
    private readonly Mock<IValidator<ProductCreateDTO>> _validatorMock;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _serviceMock = new Mock<IProductService>();
        _validatorMock = new Mock<IValidator<ProductCreateDTO>>();
        _controller = new ProductController(_serviceMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task GetAll_Should_Return_PagedResponse()
    {
        // Arrange
        var pageResult = new PagedResponse<ProductResponseDTO>
        {
            Page = 1,
            PageSize = 10,
            TotalPages = 1,
            TotalRecords = 1,
            Data = new List<ProductResponseDTO>
                {
                    new ProductResponseDTO { Id = 1, Name = "Mouse", Description = "Óptico", Price = 20, Stock = 5 }
                }
        };

        var query = new ProductQueryParamsDTO { Page = 1, PageSize = 10 };
        _serviceMock
            .Setup(s => s.GetAllAsync(null, null, null, null, 1, 10))
            .ReturnsAsync(pageResult);

        // Act
        var result = await _controller.GetAll(query);

        // Assert
        var ok = result.Result as OkObjectResult;
        ok.Should().NotBeNull();
        var response = ok!.Value as ApiResponseData<PagedResponse<ProductResponseDTO>>;
        response.Should().NotBeNull();
        response!.Data?.Page.Should().Be(1);
        response.Data?.PageSize.Should().Be(10);
        response.Data?.TotalRecords.Should().Be(1);
        response.Data?.TotalPages.Should().Be(1);
        response.Data?.Data.Should().NotBeNull();
        response.Data?.Data!.Count.Should().Be(1);
        response!.Message.Should().Be("Productos obtenidos correctamente");
        response!.Error.Should().BeFalse();
    }
    [Fact]
    public async Task GetById_Returns_Ok_When_Product_Found()
    {
        // Arrange
        var dto = new ProductResponseDTO { Id = 1, Name = "Test", Description = "test", Price = 10, Stock = 100 };
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);
        // act
        var result = await _controller.GetById(1);
        //assert
        var ok = result.Result as OkObjectResult;
        ok.Should().NotBeNull();
        var response = ok!.Value as ApiResponseData<ProductResponseDTO>;
        response!.Error.Should().BeFalse();
        response.Data!.Id.Should().Be(1);
    }
    [Fact]
    public async Task GetById_Returns_NotFound_When_Null()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((ProductResponseDTO?)null);
        // Act
        var result = await _controller.GetById(1);
        // Assert
        var notFound = result.Result as NotFoundObjectResult;
        notFound.Should().NotBeNull();
        var response = notFound!.Value as ApiResponse;
        response!.Error.Should().BeTrue();
    }
    [Fact]
    public async Task Create_Should_Return_BadRequest_When_Invalid()
    {
        // Arrange
        var dto = new ProductCreateDTO();
        var errors = new List<ValidationFailure>
            {
                new ValidationFailure("Name", "El nombre es requerido")
            };

        _validatorMock.Setup(v => v.ValidateAsync(dto, default))
    .ReturnsAsync(new ValidationResult(errors));

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var badRequest = result.Result as BadRequestObjectResult;
        badRequest.Should().NotBeNull();

        var response = badRequest!.Value as ApiResponseData<List<string>>;
        response.Should().NotBeNull();
        response!.Error.Should().BeTrue();
        response.Data!.Should().Contain("El nombre es requerido");

        _serviceMock.Verify(s => s.CreateAsync(It.IsAny<ProductCreateDTO>()), Times.Never);
    }
    [Fact]
    public async Task Create_Should_Return_BadRequest_When_Valid()
    {
        // Arrange
        var dto = new ProductCreateDTO
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 10.0m,
            Stock = 100
        };
        _validatorMock.Setup(v => v.ValidateAsync(dto, default))
    .ReturnsAsync(new ValidationResult());

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var badRequest = result.Result as OkObjectResult;
        badRequest.Should().NotBeNull();

        var response = badRequest!.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.Error.Should().BeFalse();
        _serviceMock.Verify(s => s.CreateAsync(It.IsAny<ProductCreateDTO>()), Times.Once);
    }
    [Fact]
    public async Task Update_Returns_BadRequest_When_Id_Mismatch()
    {
        //arange
        var dto = new ProductUpdateDTO { Id = 5, Name = "Updated Product", Description = "Updated Description", Price = 20.0m, Stock = 50 };
        _validatorMock.Setup(v => v.ValidateAsync(dto, default))
          .ReturnsAsync(new ValidationResult());
        //act
        var result = await _controller.Update(4, dto);
        //assert
        var badRequest = result.Result as BadRequestObjectResult;
    }
    [Fact]
    public async Task Update_Returns_NotFound_When_Product_Missing()
    {
        //arrange
        var dto = new ProductUpdateDTO { Id = 5, Name = "Updated Product", Description = "Updated Description", Price = 20.0m, Stock = 50 };
        _validatorMock.Setup(v => v.ValidateAsync(dto, default))
               .ReturnsAsync(new ValidationResult());
        _serviceMock.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(false);
        //act
        var result = await _controller.Update(5, dto);
        //assert
        var notFound = result.Result as NotFoundObjectResult;
        notFound.Should().NotBeNull();
        var response = notFound!.Value as ApiResponse;
        response!.Error.Should().BeTrue();
    }
    [Fact]
    public async Task Delete_Returns_Ok_When_Product_Deleted()
    {
        // Arrange
        _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);
        // Act
        var result = await _controller.Delete(1);
        // Assert
        var ok = result.Result as OkObjectResult;
        ok.Should().NotBeNull();
        var response = ok!.Value as ApiResponse;
        response!.Error.Should().BeFalse();
    }
    [Fact]
    public async Task Delete_Returns_NotFound_When_Missing()
    {
        _serviceMock.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);

        var result = await _controller.Delete(99);

        var notFound = result.Result as NotFoundObjectResult;
        notFound.Should().NotBeNull();
        var response = notFound!.Value as ApiResponse;
        response!.Error.Should().BeTrue();
    }
}
