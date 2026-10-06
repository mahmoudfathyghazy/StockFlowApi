using InventoryManagementSystemApi.DTOs.Products;
using InventoryManagementSystemApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return Ok(products);
        }

        // GET: api/products/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResponseDto>> GetById(int id)
        {
            var product = await _productRepository.GetProductInventorySummaryAsync(id);
            if (product == null)
            {
                return NotFound(new { message = $"Product with ID {id} not found." });
            }

            return Ok(product);
        }

        // GET: api/products/low-stock (Complex Query 2!)
        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetLowStockAlerts()
        {
            var lowStockProducts = await _productRepository.GetLowStockAlertsAsync();
            return Ok(lowStockProducts);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> Create([FromBody] ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdProduct = await _productRepository.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        // DELETE: api/products/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productRepository.DeleteProductAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = $"Product with ID {id} not found." });
            }

            return NoContent();
        }
    }
}
