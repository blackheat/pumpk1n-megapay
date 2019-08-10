using System;
using AutoMapper;
using pumpk1n_backend.Mappings;
using pumpk1n_backend.Models.Entities.Tokens;

namespace pumpk1n_backend.Models.ReturnModels.Tokens
{
    public class CoinGateBillModel : IHaveCustomMappings
    {
        public long Id { get; set; }
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
        
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<TokenBilling, CoinGateBillModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserTokenTransactionId, opt => opt.MapFrom(src => src.UserTokenTransactionId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.GatewayInvoiceId, opt => opt.MapFrom(src => src.GatewayInvoiceId))
                .ForMember(dest => dest.GatewayInvoiceReferenceLink,
                    opt => opt.MapFrom(src => src.GatewayInvoiceReferenceLink))
                .ForMember(dest => dest.ReceivedAmount, opt => opt.MapFrom(src => src.ReceivedAmount))
                .ForMember(dest => dest.InvoiceFullyPaid, opt => opt.MapFrom(src => src.InvoiceFullyPaid))
                .ForMember(dest => dest.GatewayStatus, opt => opt.MapFrom(src => src.GatewayStatus))
                .ForMember(dest => dest.GatewayInvoiceSecret, opt => opt.MapFrom(src => src.GatewayInvoiceSecret))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.CancelledDate, opt => opt.MapFrom(src => src.CancelledDate));
        }
    }
}