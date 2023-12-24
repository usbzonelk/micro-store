using AutoMapper;
using CartService.Models;
using CartService.Models.DTO;

namespace CartService
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Cart, CartDTO>().ReverseMap();
            CreateMap<Cart, UserCartItemDTO>().ReverseMap();

        }
    }

}
