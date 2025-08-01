using System;
using API_Productos.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Productos.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            // Configuración del decimal con precisión: total 18 dígitos, 2 decimales
            entity.Property(p => p.Price)
                  .HasColumnType("decimal(18,2)");

            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(p => p.Description)
                  .IsRequired()
                  .HasMaxLength(500);
        });
    }
}
