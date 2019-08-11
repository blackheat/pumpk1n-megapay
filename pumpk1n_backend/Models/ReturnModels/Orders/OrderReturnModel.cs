using System;
using System.Collections.Generic;
using AutoMapper;
using pumpk1n_backend.Mappings;
using pumpk1n_backend.Models.Entities.Orders;
using pumpk1n_backend.Models.ReturnModels.Accounts;

namespace pumpk1n_backend.Models.ReturnModels.Orders
{
    public class OrderReturnModel : IHaveCustomMappings
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public UserInformationModel RegisteredCustomerInfo { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime CheckedOutDate { get; set; }
        public DateTime ConfirmedDate { get; set; }
        public DateTime CancelledDate { get; set; }
        public List<OrderItemReturnModel> Items { get; set; } = new List<OrderItemReturnModel>();
        
        
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Order, OrderReturnModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.RegisteredCustomerInfo, opt => opt.MapFrom(src => src.Customer))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.AddedDate, opt => opt.MapFrom(src => src.AddedDate))
                .ForMember(dest => dest.CheckedOutDate, opt => opt.MapFrom(src => src.CheckedOutDate))
                .ForMember(dest => dest.ConfirmedDate, opt => opt.MapFrom(src => src.ConfirmedDate))
                .ForMember(dest => dest.CancelledDate, opt => opt.MapFrom(src => src.CancelledDate))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));
        }
    }
}