using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystemApi.Models
{
    public class Warehouse
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Location { get; set; } = string.Empty;

        // Navigation property (Many Warehouses : Many Products via WarehouseStock)
        public ICollection<WarehouseStock> WarehouseStocks { get; set; } = new List<WarehouseStock>();
    }
}
