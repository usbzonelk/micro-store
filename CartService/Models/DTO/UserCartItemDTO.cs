using System.ComponentModel.DataAnnotations;
using CartService.Models;

namespace CartService.Models.DTO
{
    public class UserCartItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

    }
}