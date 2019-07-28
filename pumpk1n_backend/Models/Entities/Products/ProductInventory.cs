using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using pumpk1n_backend.Models.Entities.Accounts;
using pumpk1n_backend.Models.Entities.Suppliers;

namespace pumpk1n_backend.Models.Entities.Products
{
    [Table("ProductInventory")]
    public class ProductInventory
    {
        [Key]
        public long Id { get; set; }
        
        public long ProductId { get; set; }
        public long SupplierId { get; set; }
        public long? CustomerId { get; set; }
        public string ProductUniqueIdentifier { get; set; }
        public DateTime ImportedDate { get; set; }
        public DateTime ExportedDate { get; set; }
        
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }
        
        [ForeignKey("CustomerId")]
        public virtual User Customer { get; set; }
    }
}