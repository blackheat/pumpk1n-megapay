using System.Linq;
using Microsoft.EntityFrameworkCore;
using pumpk1n_backend.Models.Entities.Accounts;
using pumpk1n_backend.Models.Entities.Orders;
using pumpk1n_backend.Models.Entities.Products;
using pumpk1n_backend.Models.Entities.Suppliers;
using pumpk1n_backend.Models.Entities.Tokens;

namespace pumpk1n_backend.Models.DatabaseContexts
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInventory> ProductInventories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<UserTokenTransaction> UserTokenTransactions { get; set; }
        public DbSet<TokenBilling> TokenBillings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()).ToList()
                .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
