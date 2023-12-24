using System.ComponentModel.DataAnnotations;
using OrderService.Models;

namespace OrderService.Models.DTO
{
    public class CartServiceDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

    }
}