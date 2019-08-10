using System;
using AutoMapper;
using Newtonsoft.Json;
using pumpk1n_backend.Mappings;
using pumpk1n_backend.Models.Entities.Tokens;

namespace pumpk1n_backend.Models.ReturnModels.Tokens
{
    public class CoinGateInvoiceReturnModel : IHaveCustomMappings
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("do_not_convert")]
        public bool DoNotConvert { get; set; }
        [JsonProperty("price_currency")]
        public string PriceCurrency { get; set; }
        [JsonProperty("price_amount")]
        public float PriceAmount { get; set; }
        [JsonProperty("lightning_network")]
        public bool LightningNetwork { get; set; }
        [JsonProperty("receive_currency")]
        public string ReceiveCurrency { get; set; }
        [JsonProperty("receive_amount")]
        public string ReceiveAmount { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("order_id")]
        public string OrderId { get; set; }
        [JsonProperty("payment_url")]
        public string PaymentUrl { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CoinGateInvoiceReturnModel, TokenBilling>()
                .ForMember(dest => dest.GatewayInvoiceId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.GatewayInvoiceSecret, opt => opt.MapFrom(src => src.Token))
                .ForMember(dest => dest.GatewayInvoiceReferenceLink, opt => opt.MapFrom(src => src.PaymentUrl))
                .ForMember(dest => dest.GatewayInvoiceReferenceLink, opt => opt.MapFrom(src => src.PaymentUrl))
                .ForMember(dest => dest.GatewayStatus, opt => opt.MapFrom(src => src.Status));
        }
    }
}