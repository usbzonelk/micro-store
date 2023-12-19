using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;

namespace ProductService.Repository;

public class UnitOfWork : IUnitOfWork
{
    public IProductRepository ProductRepository { get; set; }
    public IProductTypesRepository ProductTypesRepository { get; set; }
    private ProductServiceDBContext _db;

    public UnitOfWork(ProductServiceDBContext db)
    {
        _db = db;
        ProductRepository = new ProductRepository(_db);
        ProductTypesRepository = new ProductTypesRepository(_db);
    }
    public void Save()
    {
        _db.SaveChanges();
    }
}