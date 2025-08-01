using System;

namespace API_Productos.DTOs.Product;

public class ProductCreateDTO
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

