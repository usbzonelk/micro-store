using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Repository;

public interface IOrderRepository : IRepository<Order>
{
    Task RemoveMany(Expression<Func<Order, bool>> filter);
}