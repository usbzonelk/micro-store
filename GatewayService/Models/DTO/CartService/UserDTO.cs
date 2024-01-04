using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{
    public class CartUserDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

    }
}