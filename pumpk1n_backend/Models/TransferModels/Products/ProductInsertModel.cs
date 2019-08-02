using System;
using AutoMapper;
using pumpk1n_backend.Mappings;
using pumpk1n_backend.Models.Entities.Products;

namespace pumpk1n_backend.Models.TransferModels.Products
{
    public class ProductInsertModel : IHaveCustomMappings
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Image { get; set; }
        public float Price { get; set; }
        public bool Deprecated { get; set; } = false;
        
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ProductInsertModel, Product>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.ShortDescription))
                .ForMember(dest => dest.LongDescription, opt => opt.MapFrom(src => src.LongDescription))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.Deprecated, opt => opt.MapFrom(src => src.Deprecated))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));
        }
    }
}