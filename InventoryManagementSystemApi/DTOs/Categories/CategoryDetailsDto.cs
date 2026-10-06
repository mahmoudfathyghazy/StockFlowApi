namespace InventoryManagementSystemApi.DTOs.Categories
{
    public class CategoryDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        
        public List<ProductSimpleDto> Products { get; set; } = new();
    }
    public class ProductSimpleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
