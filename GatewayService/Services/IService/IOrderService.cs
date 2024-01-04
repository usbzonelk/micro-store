using GatewayService.Models.DTO;

namespace GatewayService.Services
{
    public interface IOrderService
    {
        Task<GeneralCustomAPIOutput<IEnumerable<CartDTO>>> GetCarts(string email);
        Task<GeneralCustomAPIOutput<CartDTO>> AddToCart(string email, CartProductInputDTO productToAdd);
        Task<GeneralCustomAPIOutput<bool>> RemoveFullCart(string email);

    }
}