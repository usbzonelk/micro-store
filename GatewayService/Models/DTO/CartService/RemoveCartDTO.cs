using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO
{
    public class RemoveCartDTO
    {
        public string ProductSlug { get; set; }
        public int Quantity { get; set; }

    }
}
