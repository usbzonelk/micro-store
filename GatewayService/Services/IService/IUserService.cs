using GatewayService.Models.DTO;

namespace GatewayService.Services
{
    public interface IUserService
    {
        Task<UserDTO> GetUserID(string email);//
        Task<List<UserDTO>> GetAllUsers();//
        Task<UserLoginDTO> CreateNewUser(UserLoginDTO newUserInfo); //
        Task<GeneralAPIOutput> UpdateUserPassword(UserUpdatePswDTO newUserPswInfo); //
        Task<bool> ToggleStatus(string email); //
        Task<bool> VerifyUser(VerifyUserDTO verifyInfo); //
        Task<UserRegisterDTO> AddUserDetails(UserRegisterDTO registerInfo);
        Task<bool> Authorize(UserLoginDTO userSignin);//
    }
}