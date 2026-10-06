using InventoryManagementSystemApi.Data;
using InventoryManagementSystemApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SuppliersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/suppliers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var suppliers = await _context.Suppliers
                .AsNoTracking()
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.ContactName,
                    s.Email,
                    s.Phone,
                    s.Address,
                    TotalSuppliedProducts = s.ProductSuppliers.Count()
                })
                .ToListAsync();

            return Ok(suppliers);
        }

        // GET: api/suppliers/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var supplier = await _context.Suppliers
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.ContactName,
                    s.Email,
                    s.Phone,
                    s.Address,
                    SuppliedProducts = s.ProductSuppliers.Select(ps => new
                    {
                        ps.ProductId,
                        ProductName = ps.Product != null ? ps.Product.Name : string.Empty,
                        ProductSKU = ps.Product != null ? ps.Product.SKU : string.Empty,
                        ps.SupplyPrice,
                        ps.SupplierSKU
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (supplier == null)
            {
                return NotFound(new { message = $"Supplier with ID {id} not found." });
            }

            return Ok(supplier);
        }

        // POST: api/suppliers
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Supplier supplier)
        {
            if (string.IsNullOrWhiteSpace(supplier.Name))
            {
                return BadRequest(new { message = "Supplier Name is required." });
            }

            var newSupplier = new Supplier
            {
                Name = supplier.Name,
                ContactName = supplier.ContactName,
                Email = supplier.Email,
                Phone = supplier.Phone,
                Address = supplier.Address
            };

            _context.Suppliers.Add(newSupplier);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = newSupplier.Id }, newSupplier);
        }
    }
}
