using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AdminService.Models;

namespace AdminService.Repository;

public interface IAdminRepository : IRepository<Admin>
{
}