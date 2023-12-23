using System.ComponentModel.DataAnnotations;
using UserService.Models;

namespace UserService.Models.DTO
{
    public class UserAddDetailsDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]

        public string LastName { get; set; }
        [Required]

        public string PhoneNumber { get; set; }
        [Required]

        // Address information
        public string StreetAddress { get; set; }
        [Required]

        public string City { get; set; }
        [Required]

        public string Province { get; set; }
        [Required]
        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }
    }
}