using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.Repository;

public interface IUnitOfWork
{
    IProductRepository ProductRepository { get; set; }
    IProductTypesRepository ProductTypesRepository { get; set; }
}