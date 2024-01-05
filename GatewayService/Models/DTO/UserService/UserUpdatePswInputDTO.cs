using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{
    public class UserUpdatePswInputDTO
    {

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string OldPassword { get; set; }

    }

}