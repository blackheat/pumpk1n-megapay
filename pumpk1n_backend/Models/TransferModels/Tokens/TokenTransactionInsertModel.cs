using AutoMapper;
using pumpk1n_backend.Mappings;
using pumpk1n_backend.Models.Entities.Tokens;

namespace pumpk1n_backend.Models.TransferModels.Tokens
{
    public class TokenTransactionInsertModel : IHaveCustomMappings
    {
        public float Amount { get; set; }
        public string Notes { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<TokenTransactionInsertModel, UserTokenTransaction>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes));
        }
    }
}