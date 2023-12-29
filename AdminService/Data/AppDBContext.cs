using Microsoft.EntityFrameworkCore;
using AdminService.Models;

namespace AdminService.Data
{

    public class AdminServiceDBContext : DbContext
    {
        private readonly ILogger<AdminServiceDBContext> _logger;

        public AdminServiceDBContext(ILogger<AdminServiceDBContext> logger, DbContextOptions<AdminServiceDBContext> options) : base(options)
        {
            _logger = logger;

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }

        public DbSet<Admin> Admins { get; set; }

    }

}