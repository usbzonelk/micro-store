using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using CartService.Models;

namespace CartService.Repository;

public interface IUnitOfWork
{
    ICartRepository CartRepository { set; get; }
}