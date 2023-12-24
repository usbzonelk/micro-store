using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrderService.Models;
using OrderService.Models.DTO;
using OrderService.Repository;
using OrderService.Service.IService;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Http.Features;

namespace OrderService.Controllers.v1
{
    [Route("api/v1/carts")]
    [ApiController]
    //[ApiVersion("2.0")]
    public class CartsController : ControllerBase
    {
        private readonly ILogger<CartsController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        private IProductService _productService;
        private IUserService _userService;
        private ICartService _cartService;
        public CartsController(ILogger<CartsController> logger, IUnitOfWork unitOfWork, IMapper mapper, IProductService productService, IUserService userService, ICartService cartService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
            _productService = productService;
            _userService = userService;
            _cartService = cartService;

        }
        [HttpGet("/allorders/{email}", Name = "GetAllOrders")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllOrders(string email)
        {
            IEnumerable<Order> allOrders = null;
            try
            {
                if (email == null)
                {
                    throw new Exception("Email is invalid!");
                }
                var userFound = await _userService.GetUserID(email);

                if (userFound == null || userFound.IsActive == false)
                {
                    throw new Exception("Email is invalid or account is inactive!");
                }
                int userID = userFound.UserId;

                allOrders = await _unitOfWork.OrderRepository.GetMany(order => order.UserId == userID);
                
                _response.Result = _mapper.Map<List<Order>>(allOrders);
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


    }
}