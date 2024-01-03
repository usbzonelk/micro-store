using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO
{
    public class ProductTypeDTO
    {
        [Required]
        public string TypeName { get; set; }
    }
}