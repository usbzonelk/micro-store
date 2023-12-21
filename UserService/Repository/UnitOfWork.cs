using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Repository;

namespace ProductService.Repository;

public class UnitOfWork : IUnitOfWork
{
    public IUserRepository UserRepository { get; set; }
    private UserServiceDBContext _db;

    public UnitOfWork(UserServiceDBContext db)
    {
        _db = db;
        UserRepository = new UserRepository(_db);
    }
    public void Save()
    {
        _db.SaveChanges();
    }
}