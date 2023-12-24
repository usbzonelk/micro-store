using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Repository;

namespace OrderService.Repository;

public class UnitOfWork : IUnitOfWork
{
    public IOrderRepository OrderRepository { get; set; }
    public IOrderItemRepository OrderItemRepository { get; set; }
    private OrderServiceDBContext _db;

    public UnitOfWork(OrderServiceDBContext db)
    {
        _db = db;
        OrderRepository = new OrderRepository(_db);
        OrderItemRepository = new OrderItemRepository(_db);
    }
    public void Save()
    {
        _db.SaveChanges();
    }
}