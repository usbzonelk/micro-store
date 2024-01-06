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
        public async Task<GeneralCustomAPIOutput<IEnumerable<CartOutputDTO>>> GetCarts(string email)
        {
            var output = new GeneralCustomAPIOutput<IEnumerable<CartOutputDTO>> { IsSuccessful = false, Output = new List<CartOutputDTO> { } };
            try
            {
                var client = _httpClientFactory.CreateClient("Cart");
                var response = await client.GetAsync($"/api/v1/carts/{email}");
                var apiContet = await response.Content.ReadAsStringAsync();
                Console.WriteLine("\n\n" + apiContet);
                var resp = JsonConvert.DeserializeObject<APIResponse<IEnumerable<CartOutputDTO>>>(apiContet);
                if (resp.Successful)
                {
                    output.Output = resp.Result;
                    output.IsSuccessful = true;
                }
                return output;
            }
            catch (Exception e)
            {
                return output;
            }
        }
        public async Task<GeneralCustomAPIOutput<CartOutputDTO>> AddToCart(string email, CartInputParentDTO productToAdd)
        {
            var output = new GeneralCustomAPIOutput<CartOutputDTO> { IsSuccessful = false, Output = new CartOutputDTO { } };

            var jsonPost = JsonConvert.SerializeObject(productToAdd);
            var postContent = new StringContent(jsonPost, Encoding.UTF8, "application/json");
            try
            {
                var client = _httpClientFactory.CreateClient("Cart");
                var response = await client.PostAsync($"/api/v1/carts/addtocart/{email}", postContent);
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<CartOutputDTO>>(apiContet);
                if (resp.Successful)
                {
                    var apiRes = resp.Result;
                    output.Output = apiRes;
                    output.IsSuccessful = true;
                }
                return output;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<GeneralCustomAPIOutput<string>> RemoveFullCart(string email)
        {
            var output = new GeneralCustomAPIOutput<string> { IsSuccessful = false, Output = "" };

            try
            {
                var client = _httpClientFactory.CreateClient("Cart");
                var response = await client.DeleteAsync($"/api/v1/carts/removefullcart/{email}");
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<string>>(apiContet);
                if (resp.Successful)
                {
                    output.Output = resp.Result;
                    output.IsSuccessful = true;
                }
                return output;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}