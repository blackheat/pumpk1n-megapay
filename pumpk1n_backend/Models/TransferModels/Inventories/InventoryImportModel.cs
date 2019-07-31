using System;
using AutoMapper;
using pumpk1n_backend.Mappings;
using pumpk1n_backend.Models.Entities.Products;

namespace pumpk1n_backend.Models.TransferModels.Inventories
{
    public class InventoryImportModel : IHaveCustomMappings
    {
        public long ProductId { get; set; }
        public long SupplierId { get; set; }
        public string ProductUniqueIdentifier { get; set; }
        public DateTime ImportedDate { get; set; }
        
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<InventoryImportModel, ProductInventory>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId))
                .ForMember(dest => dest.ProductUniqueIdentifier, opt => opt.MapFrom(src => src.ProductUniqueIdentifier))
                .ForMember(dest => dest.ImportedDate, opt => opt.MapFrom(src => src.ImportedDate));
        }
    }
}