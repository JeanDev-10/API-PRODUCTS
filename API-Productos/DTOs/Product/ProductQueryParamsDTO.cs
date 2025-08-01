using System;

namespace API_Productos.DTOs.Product;

public class ProductQueryParamsDTO
{
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
