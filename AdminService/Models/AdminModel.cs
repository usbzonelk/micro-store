using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace AdminService.Models
{
    [Index(nameof(Email), IsUnique = true)]

    public class Admin
    {
        [Key] public int Id { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }
        public bool IsActive { get; set; } = false;

    }

    [Index(nameof(Name), IsUnique = true)]
    public class Permission
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
    }

}