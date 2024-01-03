using GatewayService.Models.DTO;

namespace GatewayService.Services
{
    public interface IProductService
    {
        Task<GeneralCustomAPIOutput<IEnumerable<ProductDTO>>> GetProducts();
        Task<GeneralCustomAPIOutput<IEnumerable<ProductDTO>>> GetProductsOfType(string typeName);
        Task<GeneralCustomAPIOutput<IEnumerable<ProductDTO>>> SearchProducts(string query);
        Task<GeneralCustomAPIOutput<ProductDTO>> GetProduct(string slug);
        Task<GeneralCustomAPIOutput<ProductDTO>> CreateProduct(ProductInputDTO newProductDTO);
        Task<GeneralCustomAPIOutput<ProductDTO>> UpdateProduct(string slug, ProductUpdateDTO productUpdate);
        Task<GeneralCustomAPIOutput<bool>> DeleteProduct(string slug);

    }
}