using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{
    public class OrderOutputDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }

    }
}