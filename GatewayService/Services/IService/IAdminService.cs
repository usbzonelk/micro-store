using GatewayService.Models.DTO;

namespace GatewayService.Services
{
    public interface IAdminService
    {
        Task<UserDTO> GetAdminID(string email);
    }
}