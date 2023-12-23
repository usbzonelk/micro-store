using Microsoft.EntityFrameworkCore;
using CartService.Models;

namespace CartService.Data
{

    public class CartServiceDBContext : DbContext
    {
        private readonly ILogger<CartServiceDBContext> _logger;

        public CartServiceDBContext(ILogger<CartServiceDBContext> logger, DbContextOptions<CartServiceDBContext> options) : base(options)
        {
            _logger = logger;

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }

        public DbSet<Cart> ShoppingCarts { get; set; }

    }

}