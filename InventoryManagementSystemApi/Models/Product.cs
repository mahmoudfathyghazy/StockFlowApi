using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystemApi.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string SKU { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Range(0, 1000000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key & Navigation to Category (Many Products : 1 Category)
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // Many-to-Many via intermediate models
        public ICollection<ProductSupplier> ProductSuppliers { get; set; } = new List<ProductSupplier>();
        public ICollection<WarehouseStock> WarehouseStocks { get; set; } = new List<WarehouseStock>();
    }
}
