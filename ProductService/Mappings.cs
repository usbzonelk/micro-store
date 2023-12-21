using AutoMapper;
using ProductService.Models;
using ProductService.Models.DTO;

namespace ProductService
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.ProductTypeName, opt => opt.MapFrom(src => src.ProductType.TypeName));

            //ReverseMap for bidirection
            CreateMap<ProductDTO, Product>();
            CreateMap<ProductUpdateDTO, Product>();

            CreateMap<ProductInputDTO, Product>();

            CreateMap<ProductType, Product>()
           .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => new ProductType { ProductTypeID = src.ProductTypeID, TypeName = src.TypeName }));
        }
    }

}
