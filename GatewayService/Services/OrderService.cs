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

                var resp = JsonConvert.DeserializeObject<APIResponse<IEnumerable<object>>>(apiContet);
                if (resp is not null && resp.Successful)
                {
                    var resp2 = JsonConvert.DeserializeObject<APIResponse<IEnumerable<OrderOutputDTO>>>(apiContet);

                    var apiRes = resp2.Result;
                    output.Output = apiRes;
                    output.IsSuccessful = true;
                }
                else if (resp is null)
                {
                    output.IsSuccessful = true;
                }
                return output;
            }
            catch (Exception e)
            {
                return output;
            }
        }

        public async Task<GeneralCustomAPIOutput<string>> CreateOrder(string email)
        {
            var output = new GeneralCustomAPIOutput<string> { IsSuccessful = false, Output = null };
            var jsonPost = JsonConvert.SerializeObject(new object { });
            var postContent = new StringContent(jsonPost, Encoding.UTF8, "application/json");

            try
            {
                var client = _httpClientFactory.CreateClient("Order");
                var response = await client.PostAsync($"/createorder/{email}", postContent);
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<string>>(apiContet);
                if (resp.Successful)
                {
                    output.Output = (resp.Result is "Cart is empty") ? resp.Result : "Successfully checked out!";

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