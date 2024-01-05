using System.Runtime.Serialization;
using System.Text;
using GatewayService.Models.DTO;

using Newtonsoft.Json;

namespace GatewayService.Services
{
    public class AdminService : IAdminService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminService(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }
        public async Task<AdminDTO> GetAdminInfo(string email)
        {
            var client = _httpClientFactory.CreateClient("Admin");
            var response = await client.GetAsync($"/api/v1/admins/{email}");
            var apiContet = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<APIResponse>(apiContet);
            if (resp.Successful)
            {
                return JsonConvert.DeserializeObject<AdminDTO>(Convert.ToString(resp.Result));
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
            var resp = JsonConvert.DeserializeObject<APIResponse>(apiContet);
            if (resp.Successful)
            {
                return JsonConvert.DeserializeObject<AdminDTO>(Convert.ToString(resp.Result));
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
            var resp = JsonConvert.DeserializeObject<APIResponse>(apiContet);
            if (resp.Successful)
            {
                return JsonConvert.DeserializeObject<string>(Convert.ToString(resp.Result));
            }
            else
            {
                if (resp.Result is not null)
                {
                    return JsonConvert.DeserializeObject<string>(Convert.ToString(resp.Result));

                }
                else
                {
                    return JsonConvert.DeserializeObject<string>(Convert.ToString(resp.Errors));

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
            var resp = JsonConvert.DeserializeObject<APIResponse>(apiContet);
            if (resp.Successful)
            {
                return JsonConvert.DeserializeObject<string>(Convert.ToString(resp.Result));
            }
            else
            {
                var toBeSerialized = (resp.Result is not null) ? resp.Result : resp.Errors;
                return JsonConvert.DeserializeObject<string>(Convert.ToString(toBeSerialized));
            }
        }
        public async Task<string> Authorize(AdminSignupDTO adminSignin)
        {
            var client = _httpClientFactory.CreateClient("Admin");

            var jsonPost = JsonConvert.SerializeObject(adminSignin);
            var postContent = new StringContent(jsonPost, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/v1/admins/authorize", postContent);
            var apiContet = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<APIResponse>(apiContet);
            if (resp.Successful)
            {
                return Convert.ToString(resp.Result);
            }
            else
            {
                var toBeSerialized = (resp.Result is not null) ? resp.Result : resp.Errors;
                toBeSerialized = toBeSerialized is null ? "" : toBeSerialized;
                return Convert.ToString(toBeSerialized);
            }
        }
        public async Task<List<AdminDTO>> GetAllAdmins()
        {
            var client = _httpClientFactory.CreateClient("Admin");
            var response = await client.GetAsync($"/api/v1/admins/");
            var apiContet = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<APIResponse>(apiContet);
            if (resp.Successful)
            {
                return JsonConvert.DeserializeObject<List<AdminDTO>>(Convert.ToString(resp.Result));
            }
            return new List<AdminDTO>();
        }
    }
}