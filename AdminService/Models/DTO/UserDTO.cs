using System.ComponentModel.DataAnnotations;
using CartService.Models;

namespace CartService.Models.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

    }
}