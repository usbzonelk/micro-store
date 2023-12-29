using AutoMapper;
using AdminService.Models;
using AdminService.Models.DTO;

namespace AdminService
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Admin, AdminDTO>().ReverseMap();
            CreateMap<AdminInputDTO, Admin>().ReverseMap();
        }
    }

}
