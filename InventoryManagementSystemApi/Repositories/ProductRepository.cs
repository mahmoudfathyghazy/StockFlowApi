using InventoryManagementSystemApi.Data;
using InventoryManagementSystemApi.DTOs.Products;
using InventoryManagementSystemApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystemApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        // =========================================================================
        // 1. GET ALL PRODUCTS (Projected to DTO)
        // =========================================================================
        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    SKU = p.SKU,
                    Description = p.Description,
                    Price = p.Price,
                    CreatedAt = p.CreatedAt,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.Name : string.Empty,

                    TotalQuantityOnHand = p.WarehouseStocks.Sum(ws => (int?)ws.Quantity) ?? 0,
                    IsLowStock = p.WarehouseStocks.Any(ws => ws.Quantity <= ws.ReorderLevel),

                    LowestSupplyPrice = p.ProductSuppliers.Any()
                        ? p.ProductSuppliers.Min(ps => (decimal?)ps.SupplyPrice)
                        : null,

                    Warehouses = p.WarehouseStocks.Select(ws => new WarehouseStockSummaryDto
                    {
                        WarehouseId = ws.WarehouseId,
                        WarehouseName = ws.Warehouse != null ? ws.Warehouse.Name : string.Empty,
                        Quantity = ws.Quantity,
                        ReorderLevel = ws.ReorderLevel,
                        ShelfLocation = ws.ShelfLocation
                    }).ToList(),

                    Suppliers = p.ProductSuppliers.Select(ps => new ProductSupplierSummaryDto
                    {
                        SupplierId = ps.SupplierId,
                        SupplierName = ps.Supplier != null ? ps.Supplier.Name : string.Empty,
                        SupplyPrice = ps.SupplyPrice,
                        SupplierSKU = ps.SupplierSKU
                    }).ToList()
                })
                .ToListAsync();
        }

        // =========================================================================
        // 2. COMPLEX QUERY 1: Get Single Product Inventory Summary
        // =========================================================================
        public async Task<ProductResponseDto?> GetProductInventorySummaryAsync(int productId)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.Id == productId)
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    SKU = p.SKU,
                    Description = p.Description,
                    Price = p.Price,
                    CreatedAt = p.CreatedAt,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.Name : string.Empty,

                    TotalQuantityOnHand = p.WarehouseStocks.Sum(ws => (int?)ws.Quantity) ?? 0,
                    IsLowStock = p.WarehouseStocks.Any(ws => ws.Quantity <= ws.ReorderLevel),

                    LowestSupplyPrice = p.ProductSuppliers.Any()
                        ? p.ProductSuppliers.Min(ps => (decimal?)ps.SupplyPrice)
                        : null,

                    Warehouses = p.WarehouseStocks.Select(ws => new WarehouseStockSummaryDto
                    {
                        WarehouseId = ws.WarehouseId,
                        WarehouseName = ws.Warehouse != null ? ws.Warehouse.Name : string.Empty,
                        Quantity = ws.Quantity,
                        ReorderLevel = ws.ReorderLevel,
                        ShelfLocation = ws.ShelfLocation
                    }).ToList(),

                    Suppliers = p.ProductSuppliers.Select(ps => new ProductSupplierSummaryDto
                    {
                        SupplierId = ps.SupplierId,
                        SupplierName = ps.Supplier != null ? ps.Supplier.Name : string.Empty,
                        SupplyPrice = ps.SupplyPrice,
                        SupplierSKU = ps.SupplierSKU
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        // =========================================================================
        // 3. COMPLEX QUERY 2: Low Stock Alerts
        // =========================================================================
        public async Task<IEnumerable<ProductResponseDto>> GetLowStockAlertsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.WarehouseStocks.Any(ws => ws.Quantity <= ws.ReorderLevel))
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    SKU = p.SKU,
                    Description = p.Description,
                    Price = p.Price,
                    CreatedAt = p.CreatedAt,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.Name : string.Empty,

                    TotalQuantityOnHand = p.WarehouseStocks.Sum(ws => (int?)ws.Quantity) ?? 0,
                    IsLowStock = true,

                    LowestSupplyPrice = p.ProductSuppliers.Any()
                        ? p.ProductSuppliers.Min(ps => (decimal?)ps.SupplyPrice)
                        : null,

                    Warehouses = p.WarehouseStocks.Select(ws => new WarehouseStockSummaryDto
                    {
                        WarehouseId = ws.WarehouseId,
                        WarehouseName = ws.Warehouse != null ? ws.Warehouse.Name : string.Empty,
                        Quantity = ws.Quantity,
                        ReorderLevel = ws.ReorderLevel,
                        ShelfLocation = ws.ShelfLocation
                    }).ToList(),

                    Suppliers = p.ProductSuppliers.Select(ps => new ProductSupplierSummaryDto
                    {
                        SupplierId = ps.SupplierId,
                        SupplierName = ps.Supplier != null ? ps.Supplier.Name : string.Empty,
                        SupplyPrice = ps.SupplyPrice,
                        SupplierSKU = ps.SupplierSKU
                    }).ToList()
                })
                .ToListAsync();
        }

        // =========================================================================
        // 4. CREATE PRODUCT FROM DTO
        // =========================================================================
        public async Task<ProductResponseDto> CreateProductAsync(ProductCreateDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                SKU = dto.SKU,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Return fresh projected DTO
            return (await GetProductInventorySummaryAsync(product.Id))!;
        }

        // =========================================================================
        // 5. DELETE PRODUCT
        // =========================================================================
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
