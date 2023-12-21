using System.ComponentModel.DataAnnotations;
using ProductService.Models;

namespace ProductService.Models.DTO
{
    public class ProductTypeInputDTO
    {
        [Required]
        public string TypeName { get; set; }
    }
}