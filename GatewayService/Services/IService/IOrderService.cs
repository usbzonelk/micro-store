using GatewayService.Models.DTO;

namespace GatewayService.Services
{
    public interface IOrderService
    {
        Task<GeneralCustomAPIOutput<IEnumerable<OrderOutputDTO>>> GetAllOrders(string email);
        Task<GeneralCustomAPIOutput<string>> CreateOrder(string email);

    }
}