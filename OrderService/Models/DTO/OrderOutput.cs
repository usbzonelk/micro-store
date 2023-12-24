using System.ComponentModel.DataAnnotations;
using OrderService.Models;

namespace OrderService.Models.DTO
{
    public class OrderOutputDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }

    }
}