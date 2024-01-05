using AutoMapper;
using GatewayService.Models.DTO;

namespace GatewayService
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            /* CreateMap<object, AdminDTO>()
                        .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.GetType().GetProperty("isActive").GetValue(src)))
                        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => (string)src.GetType().GetProperty("email").GetValue(src)))
                        .ReverseMap(); */
        }
    }

}
