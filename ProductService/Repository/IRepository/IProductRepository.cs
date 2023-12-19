using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.Repository;

public interface IProductRepository : IRepository<Product>
{

}