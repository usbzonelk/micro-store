using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{
    public class VerifyUserDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        [Required] public string UserToken { get; set; }
    }
}