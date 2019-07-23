using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pumpk1n_backend.Models.Entities.Products
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public Int64 Id { get; set; }
        public String Name { get; set; }
        public String ShortDescription { get; set; }
        public String LongDescription { get; set; }
        public String Image { get; set; }
    }
}