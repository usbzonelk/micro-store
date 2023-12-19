using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductService.Controllers;
using ProductService.Data;
using ProductService.Models;

namespace ProductService.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private ProductServiceDBContext _db;
    public ProductRepository(ProductServiceDBContext db) : base(db)
    {
        _db = db;
    }

}