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

}