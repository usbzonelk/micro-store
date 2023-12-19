using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.Repository;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    T Get(Expression<Func<T, bool>> find);

    void Add(T entity);

    void Remove(T entity);


}

