using System.Text;
using GatewayService.Models.DTO;
using GatewayService.Services;

using Newtonsoft.Json;

namespace GatewayService.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }
        public async Task<GeneralCustomAPIOutput<IEnumerable<ProductDTO>>> GetProducts()
        {
            var output = new GeneralCustomAPIOutput<IEnumerable<ProductDTO>> { IsSuccessful = false, Output = new List<ProductDTO> { } };
            try
            {
                var client = _httpClientFactory.CreateClient("Product");
                var response = await client.GetAsync($"/api/v1/products");
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<IEnumerable<ProductDTO>>>(apiContet);
                if (resp.Successful is true)
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

        public async Task<GeneralCustomAPIOutput<IEnumerable<ProductDTO>>> GetProductsOfType(string typeName)
        {
            var output = new GeneralCustomAPIOutput<IEnumerable<ProductDTO>> { IsSuccessful = false, Output = new List<ProductDTO> { } };
            try
            {
                var client = _httpClientFactory.CreateClient("Product");
                var response = await client.GetAsync($"/api/v1/products/type/{typeName}");
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<IEnumerable<ProductDTO>>>(apiContet);
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
        public async Task<GeneralCustomAPIOutput<IEnumerable<ProductDTO>>> SearchProducts(string query)
        {
            var output = new GeneralCustomAPIOutput<IEnumerable<ProductDTO>> { IsSuccessful = false, Output = new List<ProductDTO> { } };
            try
            {
                var client = _httpClientFactory.CreateClient("Product");
                var response = await client.GetAsync($"/api/v1/products/search?query={query}");
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<IEnumerable<ProductDTO>>>(apiContet);
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
        public async Task<GeneralCustomAPIOutput<ProductDTO>> GetProduct(string slug)
        {
            var output = new GeneralCustomAPIOutput<ProductDTO> { IsSuccessful = false, Output = new ProductDTO { } };
            try
            {
                var client = _httpClientFactory.CreateClient("Product");
                var response = await client.GetAsync($"/api/v1/products/{slug}");
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<ProductDTO>>(apiContet);
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
        public async Task<GeneralCustomAPIOutput<ProductDTO>> CreateProduct(ProductInputDTO newProductDTO)
        {
            var output = new GeneralCustomAPIOutput<ProductDTO> { IsSuccessful = false, Output = new ProductDTO { } };

            var jsonPost = JsonConvert.SerializeObject(newProductDTO);
            var postContent = new StringContent(jsonPost, Encoding.UTF8, "application/json");
            try
            {
                var client = _httpClientFactory.CreateClient("Product");
                var response = await client.PostAsync($"/api/v1/products/create", postContent);
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<ProductDTO>>(apiContet);
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
        public async Task<GeneralCustomAPIOutput<ProductDTO>> UpdateProduct(string slug, ProductUpdateDTO productUpdate)
        {
            var output = new GeneralCustomAPIOutput<ProductDTO> { IsSuccessful = false, Output = new ProductDTO { } };

            var jsonPost = JsonConvert.SerializeObject(productUpdate);
            var postContent = new StringContent(jsonPost, Encoding.UTF8, "application/json");
            try
            {
                var client = _httpClientFactory.CreateClient("Product");
                var response = await client.PutAsync($"/api/v1/products/update/{slug}", postContent);
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<ProductDTO>>(apiContet);
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
        public async Task<GeneralCustomAPIOutput<bool>> DeleteProduct(string slug)
        {
            var output = new GeneralCustomAPIOutput<bool> { IsSuccessful = false, Output = false };

            try
            {
                var client = _httpClientFactory.CreateClient("Product");
                var response = await client.DeleteAsync($"/api/v1/products/delete/{slug}");
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<APIResponse<object>>(apiContet);
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