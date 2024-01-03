using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{

    public class AdminUpdatePswDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
    }
}