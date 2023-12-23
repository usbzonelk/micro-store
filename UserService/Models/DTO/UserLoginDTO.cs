using System.ComponentModel.DataAnnotations;
using UserService.Models;
namespace UserService.Models.DTO
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