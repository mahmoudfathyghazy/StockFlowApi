using InventoryManagementSystemApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystemApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<Warehouse> Warehouses => Set<Warehouse>();
        public DbSet<ProductSupplier> ProductSuppliers => Set<ProductSupplier>();
        public DbSet<WarehouseStock> WarehouseStocks => Set<WarehouseStock>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Composite Primary Key for ProductSupplier
            modelBuilder.Entity<ProductSupplier>()
                .HasKey(ps => new { ps.ProductId, ps.SupplierId });

            modelBuilder.Entity<ProductSupplier>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.ProductSuppliers)
                .HasForeignKey(ps => ps.ProductId);

            modelBuilder.Entity<ProductSupplier>()
                .HasOne(ps => ps.Supplier)
                .WithMany(s => s.ProductSuppliers)
                .HasForeignKey(ps => ps.SupplierId);

            // 2. Composite Primary Key for WarehouseStock
            modelBuilder.Entity<WarehouseStock>()
                .HasKey(ws => new { ws.ProductId, ws.WarehouseId });

            modelBuilder.Entity<WarehouseStock>()
                .HasOne(ws => ws.Product)
                .WithMany(p => p.WarehouseStocks)
                .HasForeignKey(ws => ws.ProductId);

            modelBuilder.Entity<WarehouseStock>()
                .HasOne(ws => ws.Warehouse)
                .WithMany(w => w.WarehouseStocks)
                .HasForeignKey(ws => ws.WarehouseId);

            // 3. Precision for decimal fields
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ProductSupplier>()
                .Property(ps => ps.SupplyPrice)
                .HasPrecision(18, 2);

            // 4. Seed Data
            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Devices, gadgets, and tech accessories" },
                new Category { Id = 2, Name = "Furniture", Description = "Office chairs, desks, and ergonomic setups" },
                new Category { Id = 3, Name = "Office Supplies", Description = "Stationery, paper, and desk essentials" }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Pro Laptop 15-inch",
                    SKU = "LAP-001",
                    Description = "High-performance laptop for business use",
                    Price = 1299.99m,
                    CreatedAt = seedDate,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Wireless Mouse",
                    SKU = "MOU-002",
                    Description = "Ergonomic wireless optical mouse",
                    Price = 29.99m,
                    CreatedAt = seedDate,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 3,
                    Name = "Ergonomic Mesh Chair",
                    SKU = "CHR-003",
                    Description = "Breathable mesh chair with adjustable lumbar support",
                    Price = 199.99m,
                    CreatedAt = seedDate,
                    CategoryId = 2
                }
            );

            // Seed Suppliers
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier
                {
                    Id = 1,
                    Name = "TechWorld Distribution",
                    ContactName = "Ahmed Hassan",
                    Email = "ahmed@techworld.com",
                    Phone = "+201001234567",
                    Address = "Nasr City, Cairo, Egypt"
                },
                new Supplier
                {
                    Id = 2,
                    Name = "Prime Furnishings Ltd",
                    ContactName = "Sara Ali",
                    Email = "sara@primefurnishings.com",
                    Phone = "+201009876543",
                    Address = "Smart Village, Giza, Egypt"
                }
            );

            // Seed Warehouses
            modelBuilder.Entity<Warehouse>().HasData(
                new Warehouse { Id = 1, Name = "Main Cairo Warehouse", Location = "Nasr City, Cairo" },
                new Warehouse { Id = 2, Name = "Alexandria Distribution Hub", Location = "Borg El Arab, Alexandria" }
            );

            // Seed ProductSuppliers (Many-to-Many link)
            modelBuilder.Entity<ProductSupplier>().HasData(
                new ProductSupplier { ProductId = 1, SupplierId = 1, SupplyPrice = 1050.00m, SupplierSKU = "TW-LAP-99" },
                new ProductSupplier { ProductId = 2, SupplierId = 1, SupplyPrice = 18.50m, SupplierSKU = "TW-MOU-01" },
                new ProductSupplier { ProductId = 3, SupplierId = 2, SupplyPrice = 140.00m, SupplierSKU = "PF-CHR-77" }
            );

            // Seed WarehouseStocks (Many-to-Many link)
            modelBuilder.Entity<WarehouseStock>().HasData(
                new WarehouseStock { ProductId = 1, WarehouseId = 1, Quantity = 25, ReorderLevel = 5, ShelfLocation = "A-01-03", LastUpdated = seedDate },
                new WarehouseStock { ProductId = 1, WarehouseId = 2, Quantity = 10, ReorderLevel = 3, ShelfLocation = "B-02-01", LastUpdated = seedDate },
                new WarehouseStock { ProductId = 2, WarehouseId = 1, Quantity = 120, ReorderLevel = 20, ShelfLocation = "A-01-08", LastUpdated = seedDate },
                new WarehouseStock { ProductId = 3, WarehouseId = 1, Quantity = 15, ReorderLevel = 5, ShelfLocation = "C-05-02", LastUpdated = seedDate }
            );
        }
    }
}
