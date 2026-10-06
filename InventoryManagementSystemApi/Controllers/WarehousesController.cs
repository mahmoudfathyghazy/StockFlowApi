using InventoryManagementSystemApi.Data;
using InventoryManagementSystemApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WarehousesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/warehouses
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var warehouses = await _context.Warehouses
                .AsNoTracking()
                .Select(w => new
                {
                    w.Id,
                    w.Name,
                    w.Location,
                    TotalStockUnits = w.WarehouseStocks.Sum(ws => (int?)ws.Quantity) ?? 0,
                    DistinctProductsCount = w.WarehouseStocks.Count()
                })
                .ToListAsync();

            return Ok(warehouses);
        }

        // GET: api/warehouses/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var warehouse = await _context.Warehouses
                .AsNoTracking()
                .Where(w => w.Id == id)
                .Select(w => new
                {
                    w.Id,
                    w.Name,
                    w.Location,
                    TotalStockUnits = w.WarehouseStocks.Sum(ws => (int?)ws.Quantity) ?? 0,
                    StockItems = w.WarehouseStocks.Select(ws => new
                    {
                        ws.ProductId,
                        ProductName = ws.Product != null ? ws.Product.Name : string.Empty,
                        ProductSKU = ws.Product != null ? ws.Product.SKU : string.Empty,
                        ws.Quantity,
                        ws.ReorderLevel,
                        ws.ShelfLocation,
                        IsLowStock = ws.Quantity <= ws.ReorderLevel
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (warehouse == null)
            {
                return NotFound(new { message = $"Warehouse with ID {id} not found." });
            }

            return Ok(warehouse);
        }

        // POST: api/warehouses
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Warehouse warehouse)
        {
            if (string.IsNullOrWhiteSpace(warehouse.Name) || string.IsNullOrWhiteSpace(warehouse.Location))
            {
                return BadRequest(new { message = "Name and Location are required." });
            }

            var newWarehouse = new Warehouse
            {
                Name = warehouse.Name,
                Location = warehouse.Location
            };

            _context.Warehouses.Add(newWarehouse);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = newWarehouse.Id }, newWarehouse);
        }
    }
}
