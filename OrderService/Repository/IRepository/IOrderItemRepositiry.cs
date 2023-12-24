using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Repository;

public interface IOrderItemRepository : IRepository<OrderItem>
{
    Task RemoveMany(Expression<Func<OrderItem, bool>> filter);
}