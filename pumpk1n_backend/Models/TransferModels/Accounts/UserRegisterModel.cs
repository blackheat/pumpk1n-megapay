using AutoMapper;
using pumpk1n_backend.Mappings;
using pumpk1n_backend.Models.Entities.Accounts;

namespace pumpk1n_backend.Models.TransferModels.Accounts
{
    public class UserRegisterModel : IHaveCustomMappings
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<UserRegisterModel, User>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}
