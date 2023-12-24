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
        [HttpPost("/createorder/{email}", Name = "CreateOrder")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> CreateOrder(string email)
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

                var cartItems = await _cartService.GetCartByEmail(email);
                var allProducts = await _productService.GetProducts();

                if (cartItems.Count() < 1)
                {
                    _response.Result = "Cart is empty";
                    _response.Status = HttpStatusCode.NoContent;
                    return Ok(_response);
                }
                var newOrder = new OrderDTO { UserId = userID, OrderDate = DateTime.Now };
                var orderToSave = _mapper.Map<Order>(newOrder);
                await _unitOfWork.OrderRepository.Add(orderToSave);

                List<OrderItemDTO> orderItems = new List<OrderItemDTO>();
                foreach (var cartProduct in cartItems)
                {
                    var chosenProduct = allProducts.Where(prod => prod.ProductId == cartProduct.ProductId).FirstOrDefault();
                    if (chosenProduct == null)
                    {
                        throw new Exception("The entred priduct ID is invalid");
                    }
                    else
                    {
                        var newOrderItem = new OrderItemDTO { Total = (float)chosenProduct.Price * (float)cartProduct.Quantity, ProductId = chosenProduct.ProductId, Quantity = cartProduct.Quantity };
                        orderItems.Add(newOrderItem);

                        var orderItemToSave = _mapper.Map<OrderItem>(newOrderItem);
                        orderItemToSave.ParentOrder = orderToSave;
                        await _unitOfWork.OrderItemRepository.Add(orderItemToSave);

                    }
                }
                var cartDeletion = await _cartService.DeleteCart(email);
                if (cartDeletion == null)
                {
                    throw new Exception("Cart wasn't deleted successfully!");
                }
                _response.Result = allOrders;
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