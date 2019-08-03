using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Models.Entities.Orders;
using pumpk1n_backend.Models.Entities.Tokens;

namespace pumpk1n_backend.Models.Entities.Accounts
{
    [Table("User")]
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Nonce { get; set; }
        public string HashedPassword { get; set; }
        public float Balance { get; set; } = (float) 0.0;
        
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public UserType UserType { get; set; }
        
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<UserTokenTransaction> UserTokenTransactions { get; set; }
    }
}