using System;
using AutoMapper;
using pumpk1n_backend.Mappings;
using pumpk1n_backend.Models.Entities.Accounts;

namespace pumpk1n_backend.Models.ReturnModels.Accounts
{
    public class InternalUserInformationModel : IHaveCustomMappings
    {
        public Int64 Id { get; set; }
        public String Email { get; set; }
        public String FullName { get; set; }
        public Boolean UserIsRoot { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<InternalUser, InternalUserInformationModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.UserIsRoot, opt => opt.MapFrom(src => src.UserIsRoot))
                .ForMember(dest => dest.RegisteredDate, opt => opt.MapFrom(src => src.RegisteredDate))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate));
        }
    }
}