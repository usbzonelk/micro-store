using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Repository;

public interface IUnitOfWork
{
    IOrderRepository OrderRepository { set; get; }
}