using System.ComponentModel.DataAnnotations;
using OrderService.Models;

namespace OrderService.Models.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int InStock { get; set; }
        public bool Availability { get; set; }
        public string Slug { get; set; }

    }
}