using System;

namespace pumpk1n_backend.Models.TransferModels.Tokens
{
    public class CoinGateHookTransferModel
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string Status { get; set; }
        public float PriceAmount { get; set; }
        public string PriceCurrency { get; set; }
        public float ReceiveAmount { get; set; }
        public string ReceiveCurrency { get; set; }
        public float PayAmount { get; set; }
        public string PayCurrency { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Token { get; set; }
    }
}