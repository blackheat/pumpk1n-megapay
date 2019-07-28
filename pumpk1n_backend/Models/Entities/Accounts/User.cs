using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Models.Entities.Accounts
{
    [Table("User")]
    public class User
    {
        [Key]
        public Int64 Id { get; set; }
        public String Email { get; set; }
        public String FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String Nonce { get; set; }
        public String HashedPassword { get; set; }
        
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public UserType UserType { get; set; }
    }
}