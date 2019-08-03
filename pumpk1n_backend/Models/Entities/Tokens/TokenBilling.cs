using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pumpk1n_backend.Models.Entities.Tokens
{
    [Table("TokenBilling")]
    public class TokenBilling
    {
        [Key]
        public long Id { get; set; }
        
        [ForeignKey("UserTokenTransaction")]
        public long UserTokenTransactionId { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public string GatewayInvoiceId { get; set; }
        public string GatewayInvoiceReferenceLink { get; set; }
        public double ReceivedAmount { get; set; }
        public bool InvoiceFullyPaid { get; set; }
        public string GatewayStatus { get; set; }
        public string GatewayInvoiceSecret { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CancelledDate { get; set; }
        
        public virtual UserTokenTransaction UserTokenTransaction { get; set; }
    }
}