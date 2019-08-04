using System;
using System.Collections.Generic;
using AutoMapper;
using pumpk1n_backend.Mappings;
using pumpk1n_backend.Models.Entities.Products;

namespace pumpk1n_backend.Models.TransferModels.Inventories
{
    public class InventoryProductImportModel : IHaveCustomMappings
    {
        public long ProductId { get; set; }
        public long SupplierId { get; set; }
        public string ProductUniqueIdentifier { get; set; }
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<InventoryProductImportModel, ProductInventory>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId))
                .ForMember(dest => dest.ProductUniqueIdentifier,
                    opt => opt.MapFrom(src => src.ProductUniqueIdentifier));
        }
    }

    public class InventoryImportModel
    {
        public List<InventoryProductImportModel> InventoryItems { get; set; }
        public DateTime ImportedDate { get; set; }
    }
}