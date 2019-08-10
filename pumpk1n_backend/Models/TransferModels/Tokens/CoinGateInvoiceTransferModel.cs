using Newtonsoft.Json;

namespace pumpk1n_backend.Models.TransferModels.Tokens
{
    public class CoinGateInvoiceTransferModel
    {
        [JsonProperty("order_id")]
        public string OrderId { get; set; }
        [JsonProperty("price_amount")]
        public float PriceAmount { get; set; }
        [JsonProperty("price_currency")]
        public string PriceCurrency { get; set; }
        [JsonProperty("receive_currency")]
        public string ReceiveCurrency { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("callback_url")]
        public string CallbackUrl { get; set; }
        [JsonProperty("cancel_url")]
        public string CancelUrl { get; set; }
        [JsonProperty("success_url")]
        public string SuccessUrl { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}