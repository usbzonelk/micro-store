using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{

    public class AdminSignupDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}