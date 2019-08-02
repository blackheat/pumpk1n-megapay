using System;
using AutoMapper;
using pumpk1n_backend.Mappings;
using pumpk1n_backend.Models.Entities.Products;
using pumpk1n_backend.Models.ReturnModels.Accounts;
using pumpk1n_backend.Models.ReturnModels.Products;
using pumpk1n_backend.Models.ReturnModels.Suppliers;

namespace pumpk1n_backend.Models.ReturnModels.Inventories
{
    public class InventoryReturnModel : IHaveCustomMappings
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public ProductReturnModel ProductDetails { get; set; }
        public long SupplierId { get; set; }
        public SupplierReturnModel SupplierDetails { get; set; }
        public long? CustomerId { get; set; }
        public UserInformationModel CustomerDetails { get; set; }
        public string ProductUniqueIdentifier { get; set; }
        public DateTime ImportedDate { get; set; }
        public DateTime ExportedDate { get; set; }
        
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ProductInventory, InventoryReturnModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductDetails, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId))
                .ForMember(dest => dest.SupplierDetails, opt => opt.MapFrom(src => src.Supplier))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.CustomerDetails, opt => opt.MapFrom(src => src.Customer))
                .ForMember(dest => dest.ProductUniqueIdentifier, opt => opt.MapFrom(src => src.ProductUniqueIdentifier))
                .ForMember(dest => dest.ImportedDate, opt => opt.MapFrom(src => src.ImportedDate))
                .ForMember(dest => dest.ExportedDate, opt => opt.MapFrom(src => src.ExportedDate));
        }
    }
}