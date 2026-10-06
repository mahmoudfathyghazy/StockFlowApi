using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystemApi.DTOs.Products
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Product name is required")]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "SKU is required")]
        [MaxLength(50, ErrorMessage = "SKU cannot exceed 50 characters")]
        public string SKU { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "CategoryId is required")]
        public int CategoryId { get; set; }
    }
}
