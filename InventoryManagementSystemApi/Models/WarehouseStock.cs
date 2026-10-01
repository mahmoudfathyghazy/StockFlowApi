using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystemApi.Models
{
    public class WarehouseStock
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue)]
        public int ReorderLevel { get; set; } = 5;

        [MaxLength(50)]
        public string? ShelfLocation { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
