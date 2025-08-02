using System;
using API_Productos.Context;
using API_Productos.Interfaces;
using API_Productos.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Productos.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context) => _context = context;
    public IQueryable<Product> GetAllFilteredAsync(string? name, string? description, decimal? price, int? stock)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.Contains(name));

        if (!string.IsNullOrWhiteSpace(description))
            query = query.Where(p => p.Description.Contains(description));

        if (price.HasValue)
            query = query.Where(p => p.Price == price);

        if (stock.HasValue)
            query = query.Where(p => p.Stock == stock);

        return query;
    }
    public async Task<Product?> GetByIdAsync(int id) => await _context.Products.FindAsync(id);
    public async Task AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }
     public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
