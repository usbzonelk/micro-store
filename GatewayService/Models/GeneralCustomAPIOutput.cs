using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO
{
    public class GeneralCustomAPIOutput<T> /* where T : class */
    {
        public T Output { get; set; }
        public bool IsSuccessful { get; set; }
    }
}