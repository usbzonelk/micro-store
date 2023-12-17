using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.Data
{

    public class ProductServiceDBContext : DbContext
    {
        private readonly ILogger<ProductServiceDBContext> _logger;

        public ProductServiceDBContext(ILogger<ProductServiceDBContext> logger, DbContextOptions<ProductServiceDBContext> options) : base(options)
        {
            _logger = logger;

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }

        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }

    }

}