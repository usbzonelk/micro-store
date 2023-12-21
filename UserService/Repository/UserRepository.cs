using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;

namespace UserService.Repository;

public class UserRepository : Repository<User>, IUserRepository
{
    private UserServiceDBContext _db;
    public UserRepository(UserServiceDBContext db) : base(db)
    {
        _db = db;
    }

}