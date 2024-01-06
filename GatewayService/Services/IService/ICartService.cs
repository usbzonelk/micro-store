using GatewayService.Models.DTO;

namespace GatewayService.Services
{
    public interface ICartService
    {
        Task<GeneralCustomAPIOutput<IEnumerable<CartOutputDTO>>> GetCarts(string email);
        Task<GeneralCustomAPIOutput<CartOutputDTO>> AddToCart(string email, CartInputParentDTO productToAdd);
        Task<GeneralCustomAPIOutput<string>> RemoveFullCart(string email);

    }
}