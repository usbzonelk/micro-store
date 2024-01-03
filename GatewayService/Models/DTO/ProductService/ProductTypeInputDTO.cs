using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO
{
    public class ProductTypeInputDTO
    {
        [Required]
        public string TypeName { get; set; }
    }
}