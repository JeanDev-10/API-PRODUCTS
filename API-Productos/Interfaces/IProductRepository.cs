using System;
using API_Productos.DTOs.Product;
using API_Productos.Models;

namespace API_Productos.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllFilteredAsync(string? name, string? description, decimal? price, int? stock, int page, int pageSize);
    Task<Product?> GetByIdAsync(int id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}
