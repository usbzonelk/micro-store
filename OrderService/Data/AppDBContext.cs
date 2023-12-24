using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data
{

    public class OrderServiceDBContext : DbContext
    {
        private readonly ILogger<OrderServiceDBContext> _logger;

        public OrderServiceDBContext(ILogger<OrderServiceDBContext> logger, DbContextOptions<OrderServiceDBContext> options) : base(options)
        {
            _logger = logger;

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

    }

}