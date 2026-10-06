namespace InventoryManagementSystemApi.DTOs.Products
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }

        // Category Flattened
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        // Computed Aggregations across intermediate tables
        public int TotalQuantityOnHand { get; set; }
        public decimal? LowestSupplyPrice { get; set; }
        public bool IsLowStock { get; set; }

        // Details Breakdown
        public List<WarehouseStockSummaryDto> Warehouses { get; set; } = new();
        public List<ProductSupplierSummaryDto> Suppliers { get; set; } = new();
    }

    public class WarehouseStockSummaryDto
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int ReorderLevel { get; set; }
        public string? ShelfLocation { get; set; }
    }

    public class ProductSupplierSummaryDto
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public decimal SupplyPrice { get; set; }
        public string? SupplierSKU { get; set; }
    }
}
