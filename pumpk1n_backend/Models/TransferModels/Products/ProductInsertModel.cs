using System;
using AutoMapper;
using pumpk1n_backend.Mappings;
using pumpk1n_backend.Models.Entities.Products;

namespace pumpk1n_backend.Models.TransferModels.Products
{
    public class ProductInsertModel : IHaveCustomMappings
    {
        public String Name { get; set; }
        public String ShortDescription { get; set; }
        public String LongDescription { get; set; }
        public String Image { get; set; }
        
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ProductInsertModel, Product>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.ShortDescription))
                .ForMember(dest => dest.LongDescription, opt => opt.MapFrom(src => src.LongDescription))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));
        }
    }
}