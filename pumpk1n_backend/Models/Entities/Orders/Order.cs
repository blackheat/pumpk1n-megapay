using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using pumpk1n_backend.Models.Entities.Accounts;

namespace pumpk1n_backend.Models.Entities.Orders
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public DateTime AddedDate { get; set; }
        
        [ForeignKey("CustomerId")]
        public virtual User Customer { get; set; }
        
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}