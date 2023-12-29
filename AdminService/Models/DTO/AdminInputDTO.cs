using System.ComponentModel.DataAnnotations;
using AdminService.Models;

namespace AdminService.Models.DTO
{
    public class AdminInputDTO
    {
        public string Password { get; set; }
        public string Email { get; set; }

    }
}