using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{
    public class CartInputDTO
    {
        public List<CartProductInputDTO> CartProducts { get; set; }

    }
}