using OrderService.Models;
using OrderService.Models.DTO;
using OrderService.Service.IService;
using Newtonsoft.Json;
using System.Collections;

namespace OrderService.Service.IService
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
        public async Task<string> DeleteCart(string email)
        {
            var client = _httpClientFactory.CreateClient("Cart");
            var response = await client.DeleteAsync($"/api/v1/carts/removefullcart/{email}");
            var apiContet = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<APIResponse>(apiContet);
            if (resp.Successful)
            {
                return Convert.ToString(resp.Result);
            }
            return null;
        }

    }
}