using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using CartService.Models;

namespace CartService.Repository;

public interface ICartRepository : IRepository<Cart>
{

}