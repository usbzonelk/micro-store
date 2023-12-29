using System.ComponentModel.DataAnnotations;
using AdminService.Models;

namespace AdminService.Models.DTO
{
    public class AdminPasswordUpdateDTO
    {
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string Email { get; set; }

    }
}