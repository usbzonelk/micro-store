using OrderService.Models;
using OrderService.Models.DTO;
using OrderService.Service.IService;
using Newtonsoft.Json;
using System.Collections;

namespace OrderService.Service
{
    public class CartService : ICartService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CartService(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }
        public async Task<IEnumerable<CartServiceDTO>> GetCartByEmail(string email)
        {
            var client = _httpClientFactory.CreateClient("Cart");
            var response = await client.GetAsync($"/api/v1/carts/{email}");
            var apiContet = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<APIResponse>(apiContet);
            if (resp.Successful)
            {
                return JsonConvert.DeserializeObject<IEnumerable<CartServiceDTO>>(Convert.ToString(resp.Result));
            }
            return new List<CartServiceDTO>();
        }
    }
}