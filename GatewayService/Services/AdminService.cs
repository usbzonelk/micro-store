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
    }
}