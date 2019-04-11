using System;
using AutoMapper;
using pumpk1n_backend.Mappings;
using pumpk1n_backend.Models.Entities.Accounts;

namespace pumpk1n_backend.Models.TransferModels.Accounts
{
    public class InternalUserRegisterModel : IHaveCustomMappings
    {
        public String Email { get; set; }
        public String FullName { get; set; }
        public String Password { get; set; }
        
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<InternalUserRegisterModel, InternalUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
        }
    }
}