using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;

namespace OrderService.Repository;

public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
{
    private OrderServiceDBContext _db;
    public OrderItemRepository(OrderServiceDBContext db) : base(db)
    {
        _db = db;
    }
    public async Task RemoveMany(Expression<Func<OrderItem, bool>> filter)
    {
        var deletedEntries = await dbSet.Where(filter).ToListAsync();
        dbSet.RemoveRange(deletedEntries);
        await Save();
    }
}