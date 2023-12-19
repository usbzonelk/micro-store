using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;

namespace ProductService.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ProductServiceDBContext _db;
    internal DbSet<T> dbSet;

    public Repository(ProductServiceDBContext db)
    {
        _db = db;
        dbSet = _db.Set<T>();
    }
    public void Add(T entity)
    {
        this.dbSet.Add(entity);
    }


    public T Get(Expression<Func<T, bool>> find)
    {
        return dbSet.Where(find).FirstOrDefault();
    }

    public IEnumerable<T> GetAll()
    {
        return dbSet.ToList();
    }

    public IEnumerable<T> GetMany(Expression<Func<T, bool>> find)
    {
        return dbSet.Where(find).ToList();
    }

    public void Remove(T entity)
    {
        throw new NotImplementedException();
    }

}
