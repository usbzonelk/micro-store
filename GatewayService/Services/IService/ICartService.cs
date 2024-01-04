using GatewayService.Models.DTO;

namespace GatewayService.Services
{
    public interface ICartService
    {
        Task<GeneralCustomAPIOutput<IEnumerable<CartDTO>>> GetCarts(string email);
        Task<GeneralCustomAPIOutput<CartDTO>> AddToCart(string email, CartProductInputDTO productToAdd);
        Task<GeneralCustomAPIOutput<CartDTO>> RemoveFromCart(string email, CartProductInputDTO productToAdd);
        Task<GeneralCustomAPIOutput<bool>> RemoveFullCart(string email);

    }
}