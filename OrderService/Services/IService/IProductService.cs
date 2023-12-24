using OrderService.Models.DTO;

namespace OrderService.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
    }
}