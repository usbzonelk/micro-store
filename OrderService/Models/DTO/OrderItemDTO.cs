using System.ComponentModel.DataAnnotations;
using OrderService.Models;

namespace OrderService.Models.DTO
{
    public class OrderItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float Total { get; set; }

    }
}