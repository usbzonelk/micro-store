using System.ComponentModel.DataAnnotations;
using AdminService.Models;

namespace AdminService.Models.DTO
{
    public class AdminInputDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

    }
}