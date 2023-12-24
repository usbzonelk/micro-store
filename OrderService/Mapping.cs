using AutoMapper;
using OrderService.Models;
using OrderService.Models.DTO;

namespace OrderService
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<OrderItemDTO, OrderItem>().ReverseMap();
            CreateMap<OrderDTO, Order>().ReverseMap();
        }
    }

}
