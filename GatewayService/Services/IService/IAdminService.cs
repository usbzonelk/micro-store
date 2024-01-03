using GatewayService.Models.DTO;

namespace GatewayService.Services
{
    public interface IAdminService
    {
        Task<List<AdminDTO>> GetAllAdmins();
        Task<AdminDTO> GetAdminInfo(string email);
        Task<AdminDTO> CreateNewAdmin(AdminSignupDTO newAdminInfo);
        Task<string> UpdateAdminPassword(AdminUpdatePswDTO newAdminPswInfo);
        Task<string> ToggleStatus(string email);
        Task<string> Authorize(AdminSignupDTO adminSignin);

    }
}