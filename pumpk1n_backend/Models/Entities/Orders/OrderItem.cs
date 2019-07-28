using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using pumpk1n_backend.Models.Entities.Products;

namespace pumpk1n_backend.Models.Entities.Orders
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [Key]
        public long Id { get; set; }
        
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public long Quantity { get; set; }
        public float SinglePrice { get; set; }
        
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        
        [ForeignKey("ProductInventoryId")]
        public virtual ProductInventory ProductInventory { get; set; }
    }
}