using System.Net;
using System.Text;
using System.Web;
using GatewayService.Models.DTO;
using GatewayService.Services;

using Newtonsoft.Json;

namespace GatewayService.Services
{
    public class UsersService : IUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UsersService(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }
        public async Task<UserDTO> GetUserID(string email)
        {
            var client = _httpClientFactory.CreateClient("User");
            try
            {
                var response = await client.GetAsync($"/api/v1/users/{email}");
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<UserDTO>>(apiContet);
                if (resp.Successful)
                {
                    return JsonConvert.DeserializeObject<UserDTO>(Convert.ToString(resp.Result));
                }
            }
            catch (Exception e)
            {
                return new UserDTO();
            }
            return new UserDTO();
        }
        public async Task<bool> Authorize(UserLoginDTO userSignin)
        {
            var client = _httpClientFactory.CreateClient("User");

            var jsonPost = JsonConvert.SerializeObject(userSignin);
            var postContent = new StringContent(jsonPost, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync($"/api/v1/users/authorize", postContent);
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<object>>(apiContet);
                return resp.Successful ? true : false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<List<UserDTO>> GetAllUsers()
        {
            var client = _httpClientFactory.CreateClient("User");

            try
            {
                var response = await client.GetAsync($"/api/v1/users");
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<List<UserDTO>>>(apiContet);
                if (resp.Successful)
                {
                    return JsonConvert.DeserializeObject<List<UserDTO>>(Convert.ToString(resp.Result));
                }
                return new List<UserDTO>();
            }
            catch (Exception e)
            {
                return new List<UserDTO>();
            }
        }
        public async Task<bool> VerifyUser(VerifyUserDTO verifyInfo)
        {
            var client = _httpClientFactory.CreateClient("User");
            var jsonPost = JsonConvert.SerializeObject(new object { });
            var postContent = new StringContent(jsonPost, Encoding.UTF8, "application/json");

            try
            {
                verifyInfo.UserToken = HttpUtility.UrlEncode(verifyInfo.UserToken);

                var response = await client.PatchAsync($"/api/v1/users/verify/{verifyInfo.Email}?userToken={verifyInfo.UserToken}", postContent);
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<object>>(apiContet);

                return resp.Successful ? true : false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<bool> ToggleStatus(string email)
        {
            var client = _httpClientFactory.CreateClient("User");
            var jsonPost = JsonConvert.SerializeObject(new object { });
            var postContent = new StringContent(jsonPost, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PatchAsync($"/api/v1/users/togglestatus/{email}", postContent);
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<object>>(apiContet);
                return resp.Successful ? true : false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<UserLoginDTO> CreateNewUser(UserLoginDTO newUserInfo)
        {
            var client = _httpClientFactory.CreateClient("User");

            var jsonPost = JsonConvert.SerializeObject(newUserInfo);
            var postContent = new StringContent(jsonPost, Encoding.UTF8, "application/json");
            try
            {
                var response = await client.PostAsync($"/api/v1/users/create/", postContent);
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<UserLoginDTO>>(apiContet);
                if (resp.Successful)
                {
                    return JsonConvert.DeserializeObject<UserLoginDTO>(Convert.ToString(resp.Result));
                }
                else
                {
                    return new UserLoginDTO();
                }
            }
            catch (Exception e)
            {
                return new UserLoginDTO();
            }
        }
        public async Task<GeneralAPIOutput> UpdateUserPassword(UserUpdatePswDTO newUserPswInfo)
        {
            var client = _httpClientFactory.CreateClient("User");
            var outputMsg = new GeneralAPIOutput { IsSuccessful = true };

            var jsonPost = JsonConvert.SerializeObject(newUserPswInfo);
            var postContent = new StringContent(jsonPost, Encoding.UTF8, "application/json");
            try
            {
                var response = await client.PatchAsync($"/api/v1/users/updatepassword", postContent);
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<string>>(apiContet);
                Console.WriteLine($"\n\n153\n{JsonConvert.SerializeObject(resp.Result)}\n\n");
                if (resp.Successful)
                {
                    outputMsg.Message = resp.Result;
                    return outputMsg;
                }
                else
                {
                    outputMsg.IsSuccessful = false;
                    if (resp.Result is not null)
                    {
                        outputMsg.Message = resp.Result;
                    }
                    else
                    {
                        outputMsg.Message = string.Join(", ", resp.Errors);
                    }
                    return outputMsg;
                }
            }
            catch (Exception e)
            {
                outputMsg.IsSuccessful = false;
                outputMsg.Message = e.Message;
                return outputMsg;
            }
        }
        public async Task<UserRegisterDTO> AddUserDetails(UserRegisterDTO registerInfo)
        {
            var client = _httpClientFactory.CreateClient("User");

            var jsonPost = JsonConvert.SerializeObject(registerInfo);
            var postContent = new StringContent(jsonPost, Encoding.UTF8, "application/json");
            try
            {
                var response = await client.PutAsync($"/api/v1/users/adduserdetails/{registerInfo.Email}", postContent);
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<UserRegisterDTO>>(apiContet);

                if (resp.Successful)
                {
                    return resp.Result;
                }
                else
                {
                    return new UserRegisterDTO { };
                }
            }
            catch (Exception e)
            {
                return new UserRegisterDTO { };
            }
        }
    }
}