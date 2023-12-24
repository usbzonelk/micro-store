using OrderService.Models.DTO;

namespace OrderService.Service.IService
{
    public interface ICartService
    {
        Task<IEnumerable<CartServiceDTO>> GetCartByEmail(string email);
        Task<string> DeleteCart(string email);
    }
}