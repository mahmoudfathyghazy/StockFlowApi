using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystemApi.Models
{
    public class ProductSupplier
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        [Range(0, 1000000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SupplyPrice { get; set; }

        [MaxLength(50)]
        public string? SupplierSKU { get; set; }
    }
}
