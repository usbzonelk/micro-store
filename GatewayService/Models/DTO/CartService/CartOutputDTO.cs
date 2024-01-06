using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{
    public class CartOutputDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

    }
}