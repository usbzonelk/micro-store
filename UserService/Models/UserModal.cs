using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace UserService.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public User()
        {
            this.GenerateVerificationToken();
        }
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        public string VerificationToken { get; set; }

        public bool IsVerified { get; set; }

        // Additional user information
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        // Address information
        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }

        private void GenerateVerificationToken()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var tokenData = new byte[32];
                rng.GetBytes(tokenData);
                VerificationToken = Convert.ToBase64String(tokenData);
            }
        }

    }
}