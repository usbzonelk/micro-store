using System.ComponentModel.DataAnnotations;
using UserService.Models;

namespace UserService.Models.DTO
{

    public class UserDTO
    {
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}