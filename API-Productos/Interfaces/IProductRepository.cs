using System;
using API_Productos.DTOs.Product;
using API_Productos.Models;

namespace API_Productos.Interfaces;

public interface IProductRepository
{
    IQueryable<Product> GetAllFilteredAsync(string? name, string? description, decimal? price, int? stock);
    Task<Product?> GetByIdAsync(int id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}
