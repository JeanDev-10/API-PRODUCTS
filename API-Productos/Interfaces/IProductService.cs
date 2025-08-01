using System;
using API_Productos.DTOs.ApiResponse;
using API_Productos.DTOs.Product;

namespace API_Productos.Interfaces;

public interface IProductService
{
    Task<PagedResponse<ProductResponseDTO>> GetAllAsync(string? name, string? desc, decimal? price, int? stock, int page, int pageSize);
    Task<ProductResponseDTO?> GetByIdAsync(int id);
    Task CreateAsync(ProductCreateDTO dto);
    Task<bool> UpdateAsync(ProductUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
}
