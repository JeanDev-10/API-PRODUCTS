using System;
using API_Productos.DTOs.Product;
using API_Productos.Validators.Product;
using FluentValidation.TestHelper;

namespace API_Productos.Unit_Testing.Validators;

public class ProductCreateDtoValidatorTests
{
    private readonly ProductCreateDTOValidator _validator;
    public ProductCreateDtoValidatorTests()
    {
        _validator = new ProductCreateDTOValidator();
    }
    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        //arrange
        var model = new ProductCreateDTO { Name = "", Description = "desc", Price = 10, Stock = 1 };
        //act
        var result = _validator.TestValidate(model);
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
    [Fact]
    public void Should_Have_Error_When_Price_Is_Negative()
    {
        //arrange
        var model = new ProductCreateDTO { Name = "product 1", Description = "description 1", Price = -10, Stock = 1 };
        //act
        var result = _validator.TestValidate(model);
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Should_Pass_With_Valid_Data()
    {
        //arrange
        var model = new ProductCreateDTO { Name = "product 1", Description = "description 1", Price = 20, Stock = 1 };
        //act
        var result = _validator.TestValidate(model);
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
