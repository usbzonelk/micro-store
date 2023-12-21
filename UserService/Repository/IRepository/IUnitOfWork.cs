using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Repository;

public interface IUnitOfWork
{
    IUserRepository UserRepository { set; get; }
}