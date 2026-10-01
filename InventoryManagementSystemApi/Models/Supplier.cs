using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystemApi.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? ContactName { get; set; }

        [EmailAddress]
        [MaxLength(150)]
        public string? Email { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(250)]
        public string? Address { get; set; }

        // Navigation property (Many Suppliers : Many Products via ProductSupplier)
        public ICollection<ProductSupplier> ProductSuppliers { get; set; } = new List<ProductSupplier>();
    }
}
