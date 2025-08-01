using System;
using API_Productos.DTOs.Product;
using FluentValidation;

namespace API_Productos.Validators.Product;

public class ProductCreateDTOValidator:AbstractValidator<ProductCreateDTO>
{
    public ProductCreateDTOValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("El nombre del producto es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre del producto no puede exceder los 100 caracteres.");

        RuleFor(p => p.Description)
            .NotEmpty().WithMessage("El nombre de la descripcion es obligatorio.")
            .MaximumLength(500).WithMessage("La descripción del producto no puede exceder los 500 caracteres.");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("El precio del producto debe ser mayor que cero.");
        RuleFor(p => p.Stock)
            .GreaterThan(0).WithMessage("El stock del producto debe ser mayor que cero.");
    }
}
