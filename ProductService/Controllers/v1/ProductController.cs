using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;
using ProductService.Models.DTO;
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
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public ProductsController(ILogger<ProductsController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet(Name = "GetProducts")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetProducts()
        {
            IEnumerable<Product> allProducts;
            try
            {
                allProducts = await _unitOfWork.ProductRepository.GetAll();
                _response.Result = _mapper.Map<List<ProductDTO>>(allProducts);
                _response.Status = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.Successful = false;
                _response.Errors
                     = new List<string>() { ex.ToString() };
                _response.Status = HttpStatusCode.InternalServerError;
            }
            return _response;
        }

        [HttpGet("{slug}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProduct(string slug)
        {

            Product chosenProduct;
            try
            {
                chosenProduct = await _unitOfWork.ProductRepository.Get(product => product.Slug == slug);
                _response.Result = _mapper.Map<ProductDTO>(chosenProduct);
                if (chosenProduct == null)
                {
                    _response.Status = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    _response.Status = HttpStatusCode.OK;
                    return Ok(_response);
                }

            }
            catch (Exception ex)
            {
                _response.Successful = false;
                _response.Errors
                     = new List<string>() { ex.ToString() };
                _response.Status = HttpStatusCode.InternalServerError;
            }
            return _response;
        }

        [HttpGet("search", Name = "SearchProducts")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> SearchProducts([FromQuery] string query)
        {
            IEnumerable<Product> foundProducts;
            try
            {
                foundProducts = await _unitOfWork.ProductRepository.Search(query, "Title");
                if (foundProducts != null)
                {
                    _response.Result = _mapper.Map<List<ProductDTO>>(foundProducts);
                    _response.Status = HttpStatusCode.OK;
                    return Ok(_response);
                }
                else
                {
                    _response.Status = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }


            }
            catch (Exception ex)
            {
                _response.Successful = false;
                _response.Errors
                     = new List<string>() { ex.ToString() };
                _response.Status = HttpStatusCode.InternalServerError;
            }
            return _response;
        }


    }
}