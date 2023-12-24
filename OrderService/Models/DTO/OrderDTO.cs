using OrderService.Models;

namespace OrderService.Models.DTO
{
    public class OrderDTO
    {
        public List<OrderItemDTO> OrderItems { get; set; }

    }
}