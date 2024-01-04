using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{
    public class UserCartItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

    }
}