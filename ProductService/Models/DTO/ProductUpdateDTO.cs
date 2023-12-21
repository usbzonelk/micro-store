using System.ComponentModel.DataAnnotations;
using ProductService.Models;

namespace ProductService.Models.DTO
{
    public class ProductUpdateDTO
    {
        [Required]
        public string Slug { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public bool Availability { get; set; }

        [Required]
        public int InStock { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int WarrantyMonths { get; set; }
        public int ProductTypeID { get; set; }

    }
}