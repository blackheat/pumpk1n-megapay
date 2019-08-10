using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Models.Entities.Accounts;

namespace pumpk1n_backend.Models.Entities.Tokens
{
    [Table("UserTokenTransaction")]
    public class UserTokenTransaction
    {
        [Key]
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public float Amount { get; set; }
        public string Notes { get; set; }
        public TokenTransactionType TransactionType { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ConfirmedDate { get; set; }
        public DateTime CancelledDate { get; set; }

        [ForeignKey("CustomerId")]
        public virtual User Customer { get; set; }
        
        public virtual ICollection<TokenBilling> TokenBillings { get; set; }
    }
}