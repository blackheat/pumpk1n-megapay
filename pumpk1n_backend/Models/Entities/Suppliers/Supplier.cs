using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pumpk1n_backend.Models.Entities.Suppliers
{
    [Table("Supplier")]
    public class Supplier
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Website { get; set; }
        public DateTime AddedDate { get; set; }
        
        // Deprecated
        public bool Deprecated { get; set; }
        
        public long ComputeHash()
        {
            var reprString =
                $"{Id}_{Name}_{Description}_{Address}_{Telephone}_{Website}_{AddedDate}";
            return reprString.GetHashCode();
        }
    }
}