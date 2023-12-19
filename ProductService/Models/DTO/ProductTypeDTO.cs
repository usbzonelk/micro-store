using System.ComponentModel.DataAnnotations;
using ProductService.Models;

namespace ProductService.Models.DTO
{
    public class ProductTypeDTO
    {
        [Required]
        public string TypeName { get; set; }
    }
}