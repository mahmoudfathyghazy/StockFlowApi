using InventoryManagementSystemApi.Data;
using InventoryManagementSystemApi.DTOs.Categories;
using InventoryManagementSystemApi.DTOs.Products;
using InventoryManagementSystemApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/categories
        // Returns all categories with calculated product counts (using DTO)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAll()
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    TotalProductsCount = c.Products.Count()
                })
                .ToListAsync();

            return Ok(categories);
        }

        // GET: api/categories/1
        // Returns category with its simple products list (using DTO)
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDetailsDto>> GetById(int id)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new CategoryDetailsDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Products = c.Products.Select(p => new ProductSimpleDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        SKU = p.SKU,
                        Price = p.Price
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound(new { message = $"Category with ID {id} not found." });
            }

            return Ok(category);
        }

        // POST: api/categories
        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> Create([FromBody] CategoryCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var responseDto = new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                TotalProductsCount = 0
            };

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, responseDto);
        }

        // DELETE: api/categories/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound(new { message = $"Category with ID {id} not found." });
            }

            if (category.Products.Any())
            {
                return BadRequest(new { message = "Cannot delete category that still contains products." });
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
