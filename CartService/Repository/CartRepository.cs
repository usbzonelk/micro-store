using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using CartService.Data;
using CartService.Models;
using CartService.Repository;

namespace CartService.Repository;

public class CartRepository : Repository<Cart>, ICartRepository
{
    private CartServiceDBContext _db;
    public CartRepository(CartServiceDBContext db) : base(db)
    {
        _db = db;
    }
    public async Task RemoveMany(Expression<Func<Cart, bool>> filter)
    {
        var deletedEntries = await dbSet.Where(filter).ToListAsync();
        dbSet.RemoveRange(deletedEntries);
        await Save();
    }
}