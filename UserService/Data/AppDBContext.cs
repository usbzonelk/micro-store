using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data
{

    public class UserServiceDBContext : DbContext
    {
        private readonly ILogger<UserServiceDBContext> _logger;

        public UserServiceDBContext(ILogger<UserServiceDBContext> logger, DbContextOptions<UserServiceDBContext> options) : base(options)
        {
            _logger = logger;

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }

        public DbSet<User> User { get; set; }

    }

}