using AutoMapper;
using pumpk1n_backend.Mappings;
using pumpk1n_backend.Models.Entities.Suppliers;

namespace pumpk1n_backend.Models.TransferModels.Suppliers
{
    public class SupplierInsertModel : IHaveCustomMappings
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Website { get; set; }
        
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<SupplierInsertModel, Supplier>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.Telephone))
                .ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.Website));
        }
    }
}