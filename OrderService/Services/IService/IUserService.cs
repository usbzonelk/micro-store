using OrderService.Models.DTO;

namespace OrderService.Service.IService
{
    public interface IUserService
    {
        Task<UserDTO> GetUserID(string email);
    }
}