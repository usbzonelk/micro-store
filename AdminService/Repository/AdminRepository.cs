using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AdminService.Data;
using AdminService.Models;
using AdminService.Repository;

namespace AdminService.Repository;

public class AdminRepository : Repository<Admin>, IAdminRepository
{
    private AdminServiceDBContext _db;
    public AdminRepository(AdminServiceDBContext db) : base(db)
    {
        _db = db;
    }
    public async Task RemoveMany(Expression<Func<Admin, bool>> filter)
    {
        var deletedEntries = await dbSet.Where(filter).ToListAsync();
        dbSet.RemoveRange(deletedEntries);
        await Save();
    }
}