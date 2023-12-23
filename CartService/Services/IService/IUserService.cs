using CartService.Models.DTO;

namespace CartService.Service.IService
{
    public interface IUserService
    {
        Task<UserDTO> GetUserID(string email);
    }
}