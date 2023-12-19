using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.Repository;

public class ProductRepository : IProductRepository
{
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