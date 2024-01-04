using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{
    public class CartDTO
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

    }
}