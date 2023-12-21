using AutoMapper;
using UserService.Models;
using UserService.Models.DTO;

namespace UserService
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserCreateDTO, User>().ReverseMap();
            CreateMap<PasswordUpdateDTO, User>().ReverseMap();
            CreateMap<UserUpdateDTO, User>().ReverseMap();
        }
    }

}
