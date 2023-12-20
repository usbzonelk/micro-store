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
        protected APIResponse _response;

        public ProductsController(ILogger<ProductsController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _response = new();
        }

        [HttpGet(Name = "GetProducts")]
        public async Task<ActionResult<APIResponse>> GetProducts()
        {
            IEnumerable<Product> allProducts;
            try
            {
                allProducts = await _unitOfWork.ProductRepository.GetAll();
                _response.Result = allProducts;
                _response.Status = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.Successful = false;
                _response.Errors
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public Product GetProduct(int id)
        {
            return _unitOfWork.ProductRepository.Get(product => product.ProductID == id);
        }

    }
}