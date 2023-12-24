using System.ComponentModel.DataAnnotations;
using CartService.Models;

namespace CartService.Models.DTO
{
    public class CartInputDTO
    {
        public string Email { get; set; }
        public List<ProductInputDTO> CartProducts { get; set; }

    }
}