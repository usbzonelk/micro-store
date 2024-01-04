using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{
    public class OrderDTO
    {
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}