using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace AdminService.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}