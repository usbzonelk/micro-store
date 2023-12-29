using System.ComponentModel.DataAnnotations;
using AdminService.Models;

namespace AdminService.Models.DTO
{
    public class AdminDTO
    {
        public bool IsActive { get; set; }
        public string Email { get; set; }

    }
}