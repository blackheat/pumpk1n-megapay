using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pumpk1n_backend.Models.Entities.Products
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Image { get; set; }
        public float Price { get; set; }
        public bool OutOfStock { get; set; }
        public DateTime AddedDate { get; set; }
        
        // Deprecation
        public bool Deprecated { get; set; }

        public virtual ICollection<ProductInventory> ProductInventories { get; set; }
        
        public long ComputeHash()
        {
            var reprString =
                $"{Id}_{Name}_{LongDescription}_{ShortDescription}_{Image}_{Price}_{OutOfStock}_{AddedDate}_{Deprecated}";
            return reprString.GetHashCode();
        }
    }
}