using AutoMapper;
using pumpk1n_backend.Mappings;
using pumpk1n_backend.Models.Entities.Orders;

namespace pumpk1n_backend.Models.TransferModels.Orders
{
    public class OrderItemTransferModel : IHaveCustomMappings
    {
        public long ProductId { get; set; }
        public long Quantity { get; set; }
        
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<OrderItemTransferModel, OrderItem>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
        }
    }
}