using API_Productos.DTOs.ApiResponse;
using API_Productos.DTOs.Product;
using API_Productos.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Productos.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IValidator<ProductCreateDTO> _createValidator;
        public ProductController(IProductService service, IValidator<ProductCreateDTO> createValidator)
        {
            _createValidator = createValidator;
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponseData<PagedResponse<ProductResponseDTO>>>> GetAll([FromQuery] ProductQueryParamsDTO productQueryParamsDTO)
        {
            try
            {
                var result = await _service.GetAllAsync(productQueryParamsDTO.Name, productQueryParamsDTO.Description, productQueryParamsDTO.Price, productQueryParamsDTO.Stock, productQueryParamsDTO.Page, productQueryParamsDTO.PageSize);
                return Ok(ApiResponseData<PagedResponse<ProductResponseDTO>>.Success(result, "Productos obtenidos correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Fail($"Error al obtener productos: {ex.Message}"));
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseData<ProductResponseDTO>>> GetById(int id)
        {
            try
            {
                var product = await _service.GetByIdAsync(id);
                if (product == null)
                    return NotFound(ApiResponse.Fail("Producto no encontrado"));

                return Ok(ApiResponseData<ProductResponseDTO>.Success(product));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Fail($"Error al obtener producto: {ex.Message}"));
            }
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create([FromBody] ProductCreateDTO dto)
        {
            try
            {

                var validationResult = await _createValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    var errores = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(ApiResponseData<List<string>>.Fail("Errores de validación", errores));
                }
                await _service.CreateAsync(dto);
                return Ok(ApiResponse.Success("Producto creado correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Fail($"Error al crear producto: {ex.Message}"));
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] ProductUpdateDTO dto)
        {
            try
            {
                var validationResult = await _createValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    var errores = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(ApiResponseData<List<string>>.Fail("Errores de validación", errores));
                }
                if (id != dto.Id) return BadRequest();
                var result = await _service.UpdateAsync(dto);
                if (!result)
                    return NotFound(ApiResponse.Fail("Producto no encontrado"));
                return Ok(ApiResponse.Success("Producto actualizado correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Fail($"Error al actualizar el producto: {ex.Message}"));
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                if (!result)
                    return NotFound(ApiResponse.Fail("Producto no encontrado"));

                return Ok(ApiResponse.Success("Producto eliminado correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Fail($"Error al eliminar producto: {ex.Message}"));
            }
        }
    }
}
