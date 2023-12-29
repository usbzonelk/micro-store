using GatewayService.Models.DTO;

namespace GatewayService.Services
{
    public interface IUserService
    {
        Task<UserDTO> GetUserID(string email);
    }
}