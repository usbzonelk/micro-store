using AutoMapper;
using ProductService.Models;
using ProductService.Models.DTO;

namespace ProductService
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();
        }
    }
}
