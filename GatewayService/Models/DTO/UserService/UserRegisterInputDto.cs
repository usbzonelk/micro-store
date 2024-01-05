using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{
    public class UserRegisterInputDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }
    }

}