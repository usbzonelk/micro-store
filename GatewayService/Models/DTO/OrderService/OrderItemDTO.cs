using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{
    public class OrderItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float Total { get; set; }

    }
}