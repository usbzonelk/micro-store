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
    public async Task Save()
    {
        await _db.SaveChangesAsync();

    }
    public async Task Add(T entity)
    {
        await dbSet.AddAsync(entity);
        await Save();
    }

    public async Task<T> Get(Expression<Func<T, bool>> find)
    {
        return await dbSet.Where(find).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> find)
    {
        return await dbSet.Where(find).ToListAsync();
    }

    public async Task Remove(T entity)
    {
        dbSet.Remove(entity);
        await Save();
    }

}
