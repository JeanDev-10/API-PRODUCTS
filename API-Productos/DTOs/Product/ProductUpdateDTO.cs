using System;

namespace API_Productos.DTOs.Product;

public class ProductUpdateDTO:ProductCreateDTO
{
  public int Id { get; set; }
}
