using System.ComponentModel.DataAnnotations;
using CartService.Models;

namespace CartService.Models.DTO
{
    public class CartInputDTO
    {
        public List<ProductInputDTO> CartProducts { get; set; }

    }
}