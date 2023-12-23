using System.ComponentModel.DataAnnotations;
using UserService.Models;
namespace UserService.Models.DTO
{
    public class PasswordUpdateDTO
    {
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Old Password is required.")]
        public string OldPassword { get; set; }

    }
}