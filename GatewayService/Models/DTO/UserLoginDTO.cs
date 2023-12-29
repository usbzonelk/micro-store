using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
    }
}