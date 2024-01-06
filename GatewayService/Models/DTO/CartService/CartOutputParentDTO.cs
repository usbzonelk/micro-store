using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{
    public class CartInputParentDTO
    {
        [Required] public List<CartProductInputDTO> CartProducts { get; set; }

    }
}