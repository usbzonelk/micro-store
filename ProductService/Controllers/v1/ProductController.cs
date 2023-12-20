using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;
using ProductService.Repository;
using System.Net;
using System.Text.Json;

namespace ProductService.Controllers.v1
{
    [Route("api/v1/products")]
    [ApiController]
    //[ApiVersion("2.0")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(ILogger<ProductsController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

        }

        [HttpGet(Name = "GetProducts")]
        public async IEnumerable<Product> GetProducts()
        {
            return await _unitOfWork.ProductRepository.GetAll();
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public Product GetProduct(int id)
        {
            return _unitOfWork.ProductRepository.Get(product => product.ProductID == id);
        }

    }
}