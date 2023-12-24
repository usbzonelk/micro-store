using OrderService.Models;
using OrderService.Models.DTO;
using OrderService.Service.IService;
using Newtonsoft.Json;

namespace OrderService.Service.IService
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }
        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/v1/products");
            var apiContet = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<APIResponse>(apiContet);
            if (resp.Successful)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(Convert.ToString(resp.Result));
            }
            return new List<ProductDTO>();
        }
    }
}