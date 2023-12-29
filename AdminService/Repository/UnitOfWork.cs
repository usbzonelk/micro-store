using Microsoft.EntityFrameworkCore;
using AdminService.Data;
using AdminService.Repository;

namespace AdminService.Repository;

public class UnitOfWork : IUnitOfWork
{
    public IAdminRepository AdminRepository { get; set; }
    private AdminServiceDBContext _db;

    public UnitOfWork(AdminServiceDBContext db)
    {
        _db = db;
        AdminRepository = new AdminRepository(_db);
    }
    public void Save()
    {
        _db.SaveChanges();
    }
}