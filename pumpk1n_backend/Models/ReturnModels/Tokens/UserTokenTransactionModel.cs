using System.Collections.Generic;
using AutoMapper;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Mappings;
using pumpk1n_backend.Models.Entities.Tokens;

namespace pumpk1n_backend.Models.ReturnModels.Tokens
{
    public class UserTokenTransactionModel : IHaveCustomMappings
    {
        public long Id { get; set; }
        public float Amount { get; set; }
        public string Notes { get; set; }
        public TokenTransactionType TransactionType { get; set; }
        public List<CoinGateBillModel> PaymentInvoices { get; set; } = new List<CoinGateBillModel>();
        
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<UserTokenTransaction, UserTokenTransactionModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType))
                .ForMember(dest => dest.PaymentInvoices, opt => opt.MapFrom(src => src.TokenBillings));
        }
    }
}