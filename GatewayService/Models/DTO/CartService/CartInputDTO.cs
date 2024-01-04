using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{
    public class CartInputDTO
    {
        public List<ProductInputDTO> CartProducts { get; set; }

    }
}