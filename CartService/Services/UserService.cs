using CartService.Models;
using CartService.Models.DTO;
using CartService.Service.IService;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Service
{
    public class UserService : IUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserService(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }
        public async Task<UserDTO> GetUserID(string email)
        {
            var client = _httpClientFactory.CreateClient("User");
            var response = await client.GetAsync($"/api/v1/users/{email}");
            var apiContet = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<APIResponse>(apiContet);
            if (resp.Successful)
            {
                return JsonConvert.DeserializeObject<UserDTO>(Convert.ToString(resp.Result));
            }
            return new UserDTO();
        }
    }
}