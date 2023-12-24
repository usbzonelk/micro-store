using System.ComponentModel.DataAnnotations;
using OrderService.Models;

namespace OrderService.Models.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

    }
}