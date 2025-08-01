using System;
using API_Productos.DTOs.ApiResponse;
using API_Productos.DTOs.Product;
using API_Productos.Interfaces;
using API_Productos.Models;

namespace API_Productos.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo) => _repo = repo;

    public async Task<PagedResponse<ProductResponseDTO>> GetAllAsync(string? name, string? desc, decimal? price, int? stock, int page, int pageSize)
    {
        var query = await _repo.GetAllFilteredAsync(name, desc, price, stock, page, pageSize);
        var totalRecords = query.Count();

        return new PagedResponse<ProductResponseDTO>
        {
            Data = query.Select(p => new ProductResponseDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock
            }).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
        };
    }
    public async Task<ProductResponseDTO?> GetByIdAsync(int id)
    {
        var p = await _repo.GetByIdAsync(id);
        if (p == null) return null;
        return new ProductResponseDTO
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock
        };
    }
    public async Task CreateAsync(ProductCreateDTO dto)
    {
        var p = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock
        };
        await _repo.AddAsync(p);
    }
    public async Task<bool> UpdateAsync(ProductUpdateDTO dto)
    {
        var p = await _repo.GetByIdAsync(dto.Id);
        if (p == null) return false;
        p.Name = dto.Name;
        p.Description = dto.Description;
        p.Price = dto.Price;
        p.Stock = dto.Stock;
        await _repo.UpdateAsync(p);
        return true;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var p = await _repo.GetByIdAsync(id);
        if (p == null) return false;
        await _repo.DeleteAsync(p);
        return true;
    }
}
