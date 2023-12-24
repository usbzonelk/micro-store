using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Repository;

namespace OrderService.Repository;

public class UnitOfWork : IUnitOfWork
{
    public IOrderRepository OrderRepository { get; set; }
    private OrderServiceDBContext _db;

    public UnitOfWork(OrderServiceDBContext db)
    {
        _db = db;
        OrderRepository = new CartRepository(_db);
    }
    public void Save()
    {
        _db.SaveChanges();
    }
}