using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;
using OrderService.Repository;

namespace OrderService.Repository;

public class CartRepository : Repository<Order>, IOrderRepository
{
    private OrderServiceDBContext _db;
    public CartRepository(OrderServiceDBContext db) : base(db)
    {
        _db = db;
    }
    public async Task RemoveMany(Expression<Func<Order, bool>> filter)
    {
        var deletedEntries = await dbSet.Where(filter).ToListAsync();
        dbSet.RemoveRange(deletedEntries);
        await Save();
    }
}