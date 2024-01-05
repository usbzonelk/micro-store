using System.Text;
using GatewayService.Models.DTO;
using GatewayService.Services;

using Newtonsoft.Json;

namespace GatewayService.Services
{
    public class CartServiceC : ICartService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CartServiceC(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }
        public async Task<GeneralCustomAPIOutput<IEnumerable<CartDTO>>> GetCarts(string email)
        {
            var output = new GeneralCustomAPIOutput<IEnumerable<CartDTO>> { IsSuccessful = false, Output = new List<CartDTO> { } };
            try
            {
                var client = _httpClientFactory.CreateClient("Cart");
                var response = await client.GetAsync($"/api/v1/carts/{email}");
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<IEnumerable<CartDTO>>>(apiContet);
                if (resp.Successful)
                {
                    var apiRes = JsonConvert.DeserializeObject<IEnumerable<CartDTO>>(Convert.ToString(resp.Result));
                    output.Output = apiRes;
                    output.IsSuccessful = true;
                }
                return output;
            }
            catch (Exception e)
            {
                return output;
            }
        }
        public async Task<GeneralCustomAPIOutput<CartDTO>> AddToCart(string email, CartProductInputDTO productToAdd)
        {
            var output = new GeneralCustomAPIOutput<CartDTO> { IsSuccessful = false, Output = new CartDTO { } };

            var jsonPost = JsonConvert.SerializeObject(productToAdd);
            var postContent = new StringContent(jsonPost, Encoding.UTF8, "application/json");
            try
            {
                var client = _httpClientFactory.CreateClient("Cart");
                var response = await client.PostAsync($"/api/v1/carts/addtocart/{email}", postContent);
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<CartDTO>>(apiContet);
                if (resp.Successful)
                {
                    var apiRes = JsonConvert.DeserializeObject<CartDTO>(Convert.ToString(resp.Result));
                    output.Output = apiRes;
                    output.IsSuccessful = true;
                }
                return output;
            }
            catch (Exception e)
            {
                return output;
            }
        }
        public async Task<GeneralCustomAPIOutput<bool>> RemoveFullCart(string email)
        {
            var output = new GeneralCustomAPIOutput<bool> { IsSuccessful = false, Output = false };

            try
            {
                var client = _httpClientFactory.CreateClient("Cart");
                var response = await client.DeleteAsync($"/api/v1/carts/removefullcart/{email}");
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<bool>>(apiContet);
                if (resp.Successful)
                {
                    output.Output = true;
                    output.IsSuccessful = true;
                }
                return output;
            }
            catch (Exception e)
            {
                return output;
            }
        }
    }
}