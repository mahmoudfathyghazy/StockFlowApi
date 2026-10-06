using InventoryManagementSystemApi.DTOs.Products;

namespace InventoryManagementSystemApi.Repositories
{
    public interface IProductRepository
    {
        // 1. Get all products with stock summary
        Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();

        // 2. Complex Query 1: Get single product by ID with full breakdown
        Task<ProductResponseDto?> GetProductInventorySummaryAsync(int productId);

        // 3. Complex Query 2: Get all products with low stock across any warehouse
        Task<IEnumerable<ProductResponseDto>> GetLowStockAlertsAsync();

        // 4. Create new product from DTO
        Task<ProductResponseDto> CreateProductAsync(ProductCreateDto dto);

        // 5. Delete product
        Task<bool> DeleteProductAsync(int id);
    }
}
