using GatewayService.Models.DTO;

namespace GatewayService.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
    }
}