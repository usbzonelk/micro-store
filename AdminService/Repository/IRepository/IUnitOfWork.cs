using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AdminService.Models;

namespace AdminService.Repository;

public interface IUnitOfWork
{
    IAdminRepository AdminRepository { set; get; }
}