using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.Repository;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T> Get(Expression<Func<T, bool>> find);
    Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> find);
    Task Add(T entity);
    Task Remove(T entity);
    Task<T> Update(T entity);
    Task<IEnumerable<T>> Search(string serachQuery, string searchProperty);
    public void Detach(T entity);

    Task Save();
}

