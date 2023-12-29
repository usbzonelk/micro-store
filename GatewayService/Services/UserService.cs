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
            var response = await client.GetAsync($"/api/v1/users/{email}");
            var apiContet = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<APIResponse>(apiContet);
            Console.WriteLine($"\n\n{resp.ToString()}\n\n");
            if (resp.Successful)
            {
                return JsonConvert.DeserializeObject<UserDTO>(Convert.ToString(resp.Result));
            }
            return new UserDTO();
        }
    }
}