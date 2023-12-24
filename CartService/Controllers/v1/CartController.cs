using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CartService.Models;
using CartService.Models.DTO;
using CartService.Repository;
using CartService.Service.IService;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Http.Features;

namespace UserService.Controllers.v1
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
        public CartsController(ILogger<CartsController> logger, IUnitOfWork unitOfWork, IMapper mapper, IProductService productService, IUserService userService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
            _productService = productService;
            _userService = userService;
        }

        [HttpGet("{email}", Name = "GetFullCart")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetFullCart(string email)
        {
            IEnumerable<Cart> chosenCartItems;
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
                chosenCartItems = await _unitOfWork.CartRepository.GetMany(cartItem => cartItem.UserId == userID);
                _response.Result = _mapper.Map<List<UserCartItemDTO>>(chosenCartItems);
                if (chosenCartItems == null)
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

        [HttpPost("addtocart/{email}", Name = "addToCart")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> AddToCart(string email, [FromBody] CartInputDTO cartInputData)
        {

            try
            {
                if (email == null || cartInputData == null)
                {
                    throw new Exception(message: "The data you entered is incorrect!");
                }

                var userExists = await _userService.GetUserID(email);
                if (userExists == null)
                {
                    throw new Exception(message: "The email you entred is incorrect");
                }
                var allProducts = await _productService.GetProducts();
                foreach (var cartProduct in cartInputData.CartProducts)
                {
                    if (cartProduct.Quantity < 1)
                    {
                        throw new Exception($"{cartProduct.ProductSlug} quantity must exceed 0");

                    }
                    var enteredProduct = allProducts.FirstOrDefault(prod => prod.Slug == cartProduct.ProductSlug);

                    if (enteredProduct == null)
                    {
                        throw new Exception($"{cartProduct.ProductSlug} is invalid");
                    }
                    else
                    {
                        if (!enteredProduct.Availability || enteredProduct.InStock < cartProduct.Quantity)
                        {
                            throw new Exception($"{cartProduct.ProductSlug} product is not available or stock is insufficient");
                        }
                        else
                        {
                            CartDTO cartInput = new()
                            {
                                UserId = userExists.UserId,
                                Quantity = cartProduct.Quantity,
                                ProductId = enteredProduct.ProductId
                            };

                            Cart saveCart = _mapper.Map<Cart>(cartInput);

                            var cartItem = await _unitOfWork.CartRepository.Get(item => item.UserId == cartInput.UserId && item.ProductId == cartInput.ProductId);
                            if (cartItem != null)
                            {
                                cartInput.Quantity += cartItem.Quantity;
                                await _unitOfWork.CartRepository.Update(saveCart);
                            }
                            else
                            {
                                await _unitOfWork.CartRepository.Add(saveCart);
                            }
                            _response.Result = _mapper.Map<CartDTO>(saveCart);
                            _response.Status = HttpStatusCode.Created;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _response.Successful = false;
                _response.Errors
                     = new List<string>() { ex.ToString()
};
                _response.Status = HttpStatusCode.InternalServerError;
            }
            return _response;
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("removefromcart/{email}", Name = "RemoveFromCart")]
        public async Task<ActionResult<APIResponse>> RemoveFromCart(string email, [FromBody] RemoveCartDTO cartRemoveData)
        {
            try
            {
                if (cartRemoveData == null || email == "")
                {
                    return BadRequest();
                }
                var userExists = await _userService.GetUserID(email);
                if (userExists == null)
                {
                    throw new Exception(message: "The email you entred is incorrect");
                }
                var allProducts = await _productService.GetProducts();

                if (cartRemoveData.Quantity < 1)
                {
                    throw new Exception($"{cartRemoveData.ProductSlug} quantity must exceed 0");

                }
                var enteredProduct = allProducts.FirstOrDefault(prod => prod.Slug == cartRemoveData.ProductSlug);

                if (enteredProduct == null)
                {
                    throw new Exception($"{cartRemoveData.ProductSlug} is invalid");
                }
                else
                {
                    var cartAvailability = await _unitOfWork.CartRepository.Get(cart => cart.UserId == userExists.UserId && enteredProduct.ProductId == cart.ProductId);
                    if (cartAvailability == null || 0 > (cartAvailability.Quantity - cartRemoveData.Quantity))
                    {
                        throw new Exception($"Product had not been added to the cart");
                    }
                    else
                    {
                        int newCartQty = cartAvailability.Quantity - cartRemoveData.Quantity;

                        CartDTO cartInput = new()
                        {
                            UserId = userExists.UserId,
                            Quantity = newCartQty,
                            ProductId = enteredProduct.ProductId
                        };

                        Cart saveCart = _mapper.Map<Cart>(cartInput);

                        await _unitOfWork.CartRepository.Update(saveCart);

                        _response.Result = _mapper.Map<CartDTO>(saveCart);
                        _response.Status = HttpStatusCode.Created;
                    }
                }

            }
            catch (Exception ex)
            {
                _response.Successful = false;
                _response.Errors
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        /*
                        [HttpPut("adduserdetails/{email}", Name = "AddUserDetails")]
                        [ProducesResponseType(StatusCodes.Status204NoContent)]
                        [ProducesResponseType(StatusCodes.Status400BadRequest)]
                        public async Task<ActionResult<APIResponse>> AddUserDetails(string email, [FromBody] UserAddDetailsDTO userUpdate)
                        {
                            try
                            {
                                if (userUpdate == null || email == "")
                                {
                                    return BadRequest();
                                }
                                User userToBeChanged = await _unitOfWork.UserRepository.Get(u => u.Email == email, false);

                                if (userToBeChanged == null)
                                {
                                    _response.Status = HttpStatusCode.NotFound;
                                    _response.Successful = false;
                                    _response.Errors = new List<string> { "The email you've entered is incorrect" };
                                    return NotFound(_response);
                                }

                                User model = _mapper.Map<User>(userToBeChanged);
                                _mapper.Map(userUpdate, model);
                                model.UserId = userToBeChanged.UserId;

                                var updatedUser = await _unitOfWork.UserRepository.Update(model);
                                _response.Status = HttpStatusCode.NoContent;
                                _response.Successful = true;
                                _response.Result = updatedUser;
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




                */
    }
}