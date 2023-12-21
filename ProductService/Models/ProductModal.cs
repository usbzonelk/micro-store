using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ProductService.Models
{
    [Index(nameof(Slug), IsUnique = true)]

    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [RegularExpression(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Invalid slug")]
        public string Slug { get; set; }

        [Required(ErrorMessage = "Please enter the type.")]
        public ProductType ProductType { get; set; }

        [Required(ErrorMessage = "Please enter the price.")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Please enter the title.")]
        public string Title { get; set; }

        [Required]
        public bool Availability { get; set; }

        [Required]
        public int InStock { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int WarrantyMonths { get; set; }

    }
    [Index(nameof(TypeName), IsUnique = true)]

    public class ProductType
    {
        [Key]
        public int ProductTypeID { get; set; }

        [Required(ErrorMessage = "Please enter the product type.")]
        public string TypeName { get; set; }



    }
}