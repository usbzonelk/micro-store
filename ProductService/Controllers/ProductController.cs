using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;

namespace ProductService.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly ProductServiceDBContext _context;

    public ProductsController(ILogger<ProductsController> logger, ProductServiceDBContext context)
    {
        _logger = logger;
        _context = context;

    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
       // return await _context.Products.ToListAsync<Product>();
       return new List<Product>(){};
    }

}
