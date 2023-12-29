using System.ComponentModel.DataAnnotations;
using AdminService.Models;

namespace AdminService.Models.DTO
{
    public class AdminPasswordUpdateDTO
    {
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Old Password is required.")]
        public string OldPassword { get; set; }
        
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")] public string Email { get; set; }

    }
}