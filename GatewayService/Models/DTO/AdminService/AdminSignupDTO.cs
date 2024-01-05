using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{

    public class AdminSignupDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}