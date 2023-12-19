using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductService.Controllers;
using ProductService.Data;
using ProductService.Models;

namespace ProductService.Repository;

public class ProductTypesRepository : Repository<ProductType>, IProductTypesRepository
{
    private ProductServiceDBContext _db;
    public ProductTypesRepository(ProductServiceDBContext db) : base(db)
    {
        _db = db;
    }

}