using System.ComponentModel.DataAnnotations;
namespace GatewayService.Models.DTO

{
    public class AdminDTO
    {
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public AdminDTO MapObj(object mappingObj)
        {
            return new AdminDTO
            {
                IsActive = (bool)mappingObj.GetType().GetProperty("isActive").GetValue(mappingObj),
                Email = (string)mappingObj.GetType().GetProperty("email").GetValue(mappingObj)
            };
        }
    }
}
