using AutoMapper;
using pumpk1n_backend.Mappings;
using pumpk1n_backend.Models.Entities.Orders;
using pumpk1n_backend.Models.ReturnModels.Products;

namespace pumpk1n_backend.Models.ReturnModels.Orders
{
    public class OrderItemReturnModel : IHaveCustomMappings
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public ProductReturnModel Product { get; set; }
        public long Quantity { get; set; }
        public float SinglePrice { get; set; }
        
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<OrderItem, OrderItemReturnModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.SinglePrice, opt => opt.MapFrom(src => src.SinglePrice));
        }
    }
}