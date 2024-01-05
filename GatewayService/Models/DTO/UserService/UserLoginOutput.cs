using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{
    public class UserLoginOutput
    {
        public string AuthToken { get; set; } = "";
        public bool Successful { get; set; } = false;
    }
}