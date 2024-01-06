using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GatewayService.Models.DTO;
using GatewayService.Services;
using GatewayService.Utils;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Features;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace GatewayService.PublicControllers.v1
{
    [Route("api/v1/user/manage")]
    [ApiController]
    [Authorize]
    public class UserManageController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        protected APIOutDTO _response;
        private IUserService _userService;
        private IMapper _mapper;
        public UserManageController(IMapper mapper, ILogger<AuthController> logger, IUserService userService)
        {
            _logger = logger;
            _response = new();
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("updatePassword", Name = "UpdateUserPassword")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIOutDTO>> UpdateUserPassword(UserUpdatePswInputDTO updatePassInfoInput)
        {
            try
            {
                string userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (userEmail is null)
                {
                    _response.Status = HttpStatusCode.BadRequest;
                    _response.Result = null;
                    _response.Successful = false;

                    return BadRequest(_response);
                }

                UserUpdatePswDTO updatePassInfo = new UserUpdatePswDTO()
                {
                    Password = updatePassInfoInput.Password,
                    OldPassword = updatePassInfoInput.OldPassword,
                    Email = userEmail
                };

                if (updatePassInfo is null || (updatePassInfo.Email is null) || (updatePassInfo.Password is null))
                {
                    _response.Status = HttpStatusCode.BadRequest;
                    _response.Result = null;
                    _response.Successful = false;

                    return BadRequest(_response);
                }
                var updateStatus = await _userService.UpdateUserPassword(updatePassInfo);

                if (updateStatus == null || updateStatus.IsSuccessful == false)
                {
                    _response.Status = HttpStatusCode.BadRequest;
                    _response.Result = updateStatus.Message;
                    _response.Successful = false;

                    return BadRequest(_response);
                }
                else
                {
                    _response.Status = HttpStatusCode.OK;
                    _response.Result = updateStatus.Message;
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
        [HttpPost("addDetails", Name = "AddDetails")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIOutDTO>> AddDetails(UserRegisterInputDTO userInfoInput)
        {
            try
            {
                string userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (userEmail is null)
                {
                    _response.Status = HttpStatusCode.BadRequest;
                    _response.Result = null;
                    _response.Successful = false;

                    return BadRequest(_response);
                }

                UserRegisterDTO userInfo = _mapper.Map<UserRegisterDTO>(userInfoInput);
                userInfo.Email = userEmail;

                if ((userInfo.Email is null) || (userInfo is null))
                {
                    _response.Status = HttpStatusCode.BadRequest;
                    _response.Result = null;
                    _response.Successful = false;

                    return NotFound(_response);
                }
                var userUpdateStatus = await _userService.AddUserDetails(userInfo);

                if (userUpdateStatus is null || userUpdateStatus.Email is null)
                {
                    _response.Status = HttpStatusCode.BadRequest;
                    _response.Result = null;
                    _response.Successful = false;

                    return BadRequest(_response);
                }
                else
                {
                    _response.Status = HttpStatusCode.OK;
                    _response.Result = userUpdateStatus;
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
    }

    [Route("api/v1/user/cart")]
    [ApiController]
    [Authorize]
    public class CartManageController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        protected APIOutDTO _response;
        private ICartService _cartService;
        private IMapper _mapper;
        public CartManageController(IMapper mapper, ILogger<AuthController> logger, ICartService cartService)
        {
            _logger = logger;
            _response = new();
            _cartService = cartService;
            _mapper = mapper;
        }

        [HttpGet("viewCart", Name = "viewCart")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIOutDTO>> ViewCart()
        {
            try
            {
                string userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (userEmail is null)
                {
                    _response.Status = HttpStatusCode.BadRequest;
                    _response.Result = "Invalid email!";
                    _response.Successful = false;

                    return BadRequest(_response);
                }

                var cartFound = await _cartService.GetCarts(userEmail);

                if (cartFound == null || cartFound.IsSuccessful == false)
                {
                    _response.Status = HttpStatusCode.BadRequest;
                    _response.Result = cartFound.Output;
                    _response.Successful = false;

                    return BadRequest(_response);
                }
                else
                {
                    _response.Status = HttpStatusCode.OK;
                    _response.Result = cartFound.Output;
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

        [HttpPost("addToCart", Name = "AddToCart")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIOutDTO>> AddToCart([FromBody] CartInputParentDTO productInfoInput)
        {
            try
            {
                string userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (userEmail is null)
                {
                    _response.Status = HttpStatusCode.BadRequest;
                    _response.Result = null;
                    _response.Successful = false;

                    return BadRequest(_response);
                }

                if (productInfoInput is null)
                {
                    _response.Status = HttpStatusCode.BadRequest;
                    _response.Result = null;
                    _response.Successful = false;

                    return NotFound(_response);
                }
                var cartUpdateStatus = await _cartService.AddToCart(userEmail, productInfoInput);

                if (cartUpdateStatus is null)
                {
                    _response.Status = HttpStatusCode.BadRequest;
                    _response.Result = null;
                    _response.Successful = false;

                    return BadRequest(_response);
                }
                else
                {
                    if (cartUpdateStatus.IsSuccessful)
                    {
                        _response.Status = HttpStatusCode.OK;
                        _response.Result = cartUpdateStatus;
                        return Ok(_response);
                    }
                    else
                    {
                        _response.Status = HttpStatusCode.BadRequest;
                        _response.Result = null;
                        _response.Successful = false;
                    }
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
        [HttpDelete("deleteCart", Name = "DeleteCart")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIOutDTO>> DeleteCart()
        {
            try
            {
                string userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (userEmail is null)
                {
                    _response.Status = HttpStatusCode.BadRequest;
                    _response.Result = null;
                    _response.Successful = false;

                    return BadRequest(_response);
                }


                var cartDeleteStatus = await _cartService.RemoveFullCart(userEmail);

                if (cartDeleteStatus is null)
                {
                    _response.Status = HttpStatusCode.BadRequest;
                    _response.Result = null;
                    _response.Successful = false;

                    return BadRequest(_response);
                }
                else
                {
                    if (cartDeleteStatus.IsSuccessful)
                    {
                        _response.Status = HttpStatusCode.OK;
                        _response.Result = cartDeleteStatus;
                        return Ok(_response);
                    }
                    else
                    {
                        _response.Status = HttpStatusCode.BadRequest;
                        _response.Result = null;
                        _response.Successful = false;
                    }
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