using Microsoft.EntityFrameworkCore;
using CartService.Data;
using CartService.Repository;

namespace CartService.Repository;

public class UnitOfWork : IUnitOfWork
{
    public ICartRepository CartRepository { get; set; }
    private CartServiceDBContext _db;

    public UnitOfWork(CartServiceDBContext db)
    {
        _db = db;
        CartRepository = new CartRepository(_db);
    }
    public void Save()
    {
        _db.SaveChanges();
    }
}