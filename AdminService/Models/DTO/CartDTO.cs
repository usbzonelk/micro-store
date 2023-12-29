using System.ComponentModel.DataAnnotations;
using CartService.Models;

namespace CartService.Models.DTO
{
    public class CartDTO
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

    }
}