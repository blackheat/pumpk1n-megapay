using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pumpk1n_backend.Models.Entities.Accounts
{
    [Table("InternalUser")]
    public class InternalUser
    {
        [Key]
        public Int64 Id { get; set; }
        public String Email { get; set; }
        public String FullName { get; set; }
        public String Nonce { get; set; }
        public String HashedPassword { get; set; }
        
        public Boolean UserIsRoot { get; set; }
        
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}