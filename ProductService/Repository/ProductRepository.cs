using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductService.Controllers;
using ProductService.Data;
using ProductService.Models;

namespace ProductService.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ProductServiceDBContext _db;
    internal DbSet<Product> dbSet;

    public ProductRepository(ProductServiceDBContext db)
    {
        _db = db;
        this.dbSet = _db.Set<Product>();
    }
    public void Add(Product product)
    {
        this.dbSet.Add(product);
    }

    public Product Get(Expression<Func<Product, bool>> find)
    {
        return dbSet.Where(find).FirstOrDefault();
    }

    public IEnumerable<Product> GetAll()
    {
        return dbSet.ToList();
    }

    public IEnumerable<Product> GetMany(Expression<Func<Product, bool>> find)
    {
        return dbSet.Where(find).ToList();
    }

    public void Remove(Product product)
    {
        dbSet.Remove(product);
    }
}