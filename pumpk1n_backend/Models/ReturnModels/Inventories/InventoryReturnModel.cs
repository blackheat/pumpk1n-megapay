using System;
using AutoMapper;
using pumpk1n_backend.Mappings;
using pumpk1n_backend.Models.Entities.Products;

namespace pumpk1n_backend.Models.ReturnModels.Inventories
{
    public class InventoryReturnModel : IHaveCustomMappings
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long SupplierId { get; set; }
        public long? CustomerId { get; set; }
        public string ProductUniqueIdentifier { get; set; }
        public DateTime ImportedDate { get; set; }
        public DateTime ExportedDate { get; set; }
        
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ProductInventory, InventoryReturnModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.ProductUniqueIdentifier, opt => opt.MapFrom(src => src.ProductUniqueIdentifier))
                .ForMember(dest => dest.ImportedDate, opt => opt.MapFrom(src => src.ImportedDate))
                .ForMember(dest => dest.ExportedDate, opt => opt.MapFrom(src => src.ExportedDate));
        }
    }
}