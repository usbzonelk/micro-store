using System.ComponentModel.DataAnnotations;
using CartService.Models;

namespace CartService.Models.DTO
{
    public class ProductInputDTO
    {
        public string ProductSlug { get; set; }
        public int Quantity { get; set; }

    }
}