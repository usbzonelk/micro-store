using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{
    public class CartProductInputDTO
    {
        public string ProductSlug { get; set; }
        public int Quantity { get; set; }

    }
}