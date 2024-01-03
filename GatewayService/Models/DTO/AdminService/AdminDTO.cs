using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{

    public class AdminDTO
    {
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}