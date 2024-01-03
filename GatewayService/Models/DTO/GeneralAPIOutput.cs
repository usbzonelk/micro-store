using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO
{
    public class GeneralAPIOutput
    {
        public string Message { get; set; }
        public bool IsSuccessful { get; set; }
    }
}