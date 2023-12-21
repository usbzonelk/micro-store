using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
namespace UserService.Models
{
    public class User
    {
        public User()
        {
            this.GenerateVerificationToken();
        }
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string Username { get; set; }

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

        [DataType(DataType.Date)]

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