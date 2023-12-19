using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductService.Controllers;
using ProductService.Data;
using ProductService.Models;

namespace ProductService.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ProductServiceDBContext _db;

    public ProductRepository(ProductServiceDBContext db)
    {
        _db = db;
    }
    public void Add(Product entity)
    {
        throw new NotImplementedException();
    }

    public Product Get(Expression<Func<Product, bool>> find)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Product> GetAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Product> GetMany(Expression<Func<IEnumerable<Product>, bool>> find)
    {
        throw new NotImplementedException();
    }

    public void Remove(Product entity)
    {
        throw new NotImplementedException();
    }
}