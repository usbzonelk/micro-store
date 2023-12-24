using OrderService.Models;

namespace OrderService.Models.DTO
{
    public class OrderDTO
    {
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}