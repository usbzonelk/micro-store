using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Repository;

public interface IOrderItemRepositiry : IRepository<OrderItem>
{
    Task RemoveMany(Expression<Func<Order, bool>> filter);
}