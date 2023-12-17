using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ProductService.Models
{
    [Index(nameof(Slug), IsUnique = true)]

    public class Product
    {
        [Required(ErrorMessage = "Please enter the slug.")]
        public string Slug;

        [Required(ErrorMessage = "Please enter the type.")]
        public string? ProduductType;

        [Required(ErrorMessage = "Please enter the price.")]
        public float Price;

        [Required(ErrorMessage = "Please enter the title.")]
        public string Title;

        [Required]
        public bool Availability = true;

        [Required]
        public int InStock;

        [Required]
        public string Description;

        [Required]
        public int WarraentyMonths;

    }
    [Index(nameof(TypeName), IsUnique = true)]

    public class ProductType
    {
        [Required(ErrorMessage = "Please enter the product type.")]
        public string TypeName;



    }
}