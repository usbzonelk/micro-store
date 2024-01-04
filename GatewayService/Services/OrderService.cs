using System.Text;
using GatewayService.Models.DTO;
using GatewayService.Services;

using Newtonsoft.Json;

namespace GatewayService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderService(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }
        public async Task<GeneralCustomAPIOutput<IEnumerable<OrderOutputDTO>>> GetAllOrders(string email)
        {
            var output = new GeneralCustomAPIOutput<IEnumerable<OrderOutputDTO>> { IsSuccessful = false, Output = new List<OrderOutputDTO> { } };
            try
            {
                var client = _httpClientFactory.CreateClient("Order");
                var response = await client.GetAsync($"/getAllOrders/{email}");
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse>(apiContet);
                if (resp.Successful)
                {
                    var apiRes = JsonConvert.DeserializeObject<IEnumerable<OrderOutputDTO>>(Convert.ToString(resp.Result));
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

        public async Task<GeneralCustomAPIOutput<bool>> CreateOrder(string email)
        {
            var output = new GeneralCustomAPIOutput<bool> { IsSuccessful = false, Output = false };
            var jsonPost = JsonConvert.SerializeObject(new object { });
            var postContent = new StringContent(jsonPost, Encoding.UTF8, "application/json");

            try
            {
                var client = _httpClientFactory.CreateClient("Product");
                var response = await client.PostAsync($"/createorder/{email}", postContent);
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse>(apiContet);
                if (resp.Successful)
                {
                    output.Output = true;
                    output.IsSuccessful = (resp.Result is "Cart is empty") ? false : true;

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