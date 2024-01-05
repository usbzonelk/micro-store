using System.Runtime.Serialization;
using System.Text;
using GatewayService.Models.DTO;
using AutoMapper;

using Newtonsoft.Json;

namespace GatewayService.Services
{
    public class AdminService : IAdminService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;

        public AdminService(IHttpClientFactory clientFactory, IMapper mapper)
        {
            _httpClientFactory = clientFactory;
            _mapper = mapper;
        }
        public async Task<AdminDTO> GetAdminInfo(string email)
        {
            var client = _httpClientFactory.CreateClient("Admin");
            var response = await client.GetAsync($"/api/v1/admins/{email}");
            var apiContet = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<APIResponse<AdminDTO>>(apiContet);
            if (resp.Successful)
            {
                return resp.Result;
            }
            return new AdminDTO();
        }
        public async Task<AdminDTO> CreateNewAdmin(AdminSignupDTO newAdminInfo)
        {
            var client = _httpClientFactory.CreateClient("Admin");

            var jsonPost = JsonConvert.SerializeObject(newAdminInfo);
            var postContent = new StringContent(jsonPost, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/v1/admins/create", postContent);
            var apiContet = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<APIResponse<AdminDTO>>(apiContet);
            if (resp.Successful)
            {
                return _mapper.Map<AdminDTO>(resp.Result);
            }
            return new AdminDTO();
        }

        public async Task<string> UpdateAdminPassword(AdminUpdatePswDTO newAdminPswInfo)
        {
            var client = _httpClientFactory.CreateClient("Admin");

            var jsonPost = JsonConvert.SerializeObject(newAdminPswInfo);
            var postContent = new StringContent(jsonPost, Encoding.UTF8, "application/json");

            var response = await client.PatchAsync($"/api/v1/admins/updatepassword", postContent);
            var apiContet = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<APIResponse<string>>(apiContet);
            if (resp.Successful)
            {
                return Convert.ToString(resp.Result);
            }
            else
            {
                if (resp.Result is not null)
                {
                    return Convert.ToString(resp.Result);
                }
                else
                {
                    return Convert.ToString(resp.Errors);
                }
            }
        }

        public async Task<string> ToggleStatus(string email)
        {
            var client = _httpClientFactory.CreateClient("Admin");

            var jsonPost = JsonConvert.SerializeObject(new object { });
            var postContent = new StringContent(jsonPost, Encoding.UTF8, "application/json");

            var response = await client.PatchAsync($"/api/v1/admins/togglestatus/{email}", postContent);
            var apiContet = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<APIResponse<string>>(apiContet);
            if (resp.Successful)
            {
                return Convert.ToString(resp.Result);
            }
            else
            {
                if (resp.Result is null)
                {
                    return Convert.ToString(resp.Errors);
                }
                return Convert.ToString(resp.Result);
            }
        }
        public async Task<string> Authorize(AdminSignupDTO adminSignin)
        {
            var client = _httpClientFactory.CreateClient("Admin");

            var jsonPost = JsonConvert.SerializeObject(adminSignin);
            var postContent = new StringContent(jsonPost, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/v1/admins/authorize", postContent);
            var apiContet = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<APIResponse<string>>(apiContet);
            if (resp.Successful)
            {
                return Convert.ToString(resp.Result);
            }
            else
            {
                if (resp.Result is null)
                {
                    return Convert.ToString(resp.Errors);
                }
                return Convert.ToString(resp.Result);
            }
        }
        public async Task<List<AdminDTO>> GetAllAdmins()
        {
            var client = _httpClientFactory.CreateClient("Admin");
            var response = await client.GetAsync($"/api/v1/admins/");
            var apiContet = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<APIResponse<List<AdminDTO>>>(apiContet);
            if (resp.Successful)
            {
                return _mapper.Map<List<AdminDTO>>(resp.Result);
            }
            return new List<AdminDTO>();
        }
    }
}