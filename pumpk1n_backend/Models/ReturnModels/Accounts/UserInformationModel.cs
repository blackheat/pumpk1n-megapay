using System;
using AutoMapper;
using pumpk1n_backend.Mappings;
using pumpk1n_backend.Models.Entities.Accounts;

namespace pumpk1n_backend.Models.ReturnModels.Accounts
{
    public class UserInformationModel : IHaveCustomMappings
    {
        public Int64 Id { get; set; }
        public String GoogleOAuthProfileId { get; set; }
        public String FullName { get; set; }
        public String DateOfBirth { get; set; }
        public String PhoneNumber { get; set; }
        public DateTime? PhoneNumberConfirmedDate { get; set; }
        public String Email { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public Boolean UserProfileCompleted { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<User, UserInformationModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.GoogleOAuthProfileId, opt => opt.MapFrom(src => src.GoogleOAuthProfileId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.PhoneNumberConfirmedDate, opt => opt.MapFrom(src => src.PhoneNumberConfirmedDate))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.RegisteredDate, opt => opt.MapFrom(src => src.RegisteredDate))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate))
                .ForMember(dest => dest.ActivatedDate, opt => opt.MapFrom(src => src.ActivatedDate))
                .ForMember(dest => dest.UserProfileCompleted, opt => opt.MapFrom(src => src.UserProfileCompleted));
        }
    }
}
