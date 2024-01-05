using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using GatewayService;
using GatewayService.Models.DTO;
using GatewayService.Services;
using GatewayService.Utils;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Http.Features;
using System.Security.Claims;
using Newtonsoft.Json;

namespace GatewayService.PublicControllers.v1
{
    [Route("api/v1/public")]
    [ApiController]
    //[ApiVersion("2.0")]
    public class CartsController : ControllerBase
    {
        private readonly ILogger<CartsController> _logger;
        protected APIOutDTO _response;
        private readonly IMapper _mapper;
        private IProductService _productService;
        private IUserService _userService;
        private IAdminService _adminService;
        private ICartService _cartService;
        private IOrderService _orderService;
        public CartsController(ILogger<CartsController> logger, IMapper mapper, IProductService productService, IUserService userService, IAdminService adminService, ICartService cartService, IOrderService orderService)
        {
            _logger = logger;
            _mapper = mapper;
            _response = new();
            _productService = productService;
            _userService = userService;
            _adminService = adminService;
            _cartService = cartService;
            _orderService = orderService;
        }

        [HttpPost("user", Name = "LogInUser")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIOutDTO>> LogInUser(UserLoginDTO userInfo)
        {
            UserLoginOutput userLoginOutput = new();
            try
            {
                if ((userInfo.Email is null) || (userInfo.Password is null))
                {
                    _response.Status = HttpStatusCode.NotFound;
                    _response.Result = userLoginOutput;
                    _response.Successful = false;

                    return NotFound(_response);
                }
                var userFound = await _userService.Authorize(userInfo);

                if (userFound == null || userFound == false)
                {
                    _response.Status = HttpStatusCode.NotFound;
                    _response.Result = userLoginOutput;
                    _response.Successful = false;

                    return NotFound(_response);
                }
                else
                {
                    IEnumerable<Claim> jwtData = new List<Claim> {
                        new Claim(ClaimTypes.Email,userInfo.Email ),
                        new Claim(ClaimTypes.Role, "User"),
                    };
                    DateTime expTime = DateTime.Now.AddDays(15);
                    string jwtToken = JWTManager.GenerateJwt(jwtData, expTime);

                    _response.Status = HttpStatusCode.OK;
                    _response.Result = jwtToken;
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
        [HttpPost("admin", Name = "LogInAdmin")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIOutDTO>> LogInAdmin(AdminSignupDTO adminInfo)
        {
            UserLoginOutput userLoginOutput = new();
            try
            {
                if ((adminInfo.Email is null) || (adminInfo.Password is null))
                {
                    _response.Status = HttpStatusCode.NotFound;
                    _response.Result = userLoginOutput;
                    _response.Successful = false;

                    return NotFound(_response);
                }
                var adminFound = await _adminService.Authorize(adminInfo);

                if (adminFound is null || adminFound is not "Admin authenticated successfully!")
                {
                    _response.Status = HttpStatusCode.NotFound;
                    _response.Result = userLoginOutput;
                    _response.Successful = false;

                    return NotFound(_response);
                }
                else
                {
                    IEnumerable<Claim> jwtData = new List<Claim> {
                        new Claim(ClaimTypes.Email,adminInfo.Email ),
                        new Claim(ClaimTypes.Role, "Admin"),
                    };
                    DateTime expTime = DateTime.Now.AddDays(15);
                    string jwtToken = JWTManager.GenerateJwt(jwtData, expTime);

                    _response.Status = HttpStatusCode.OK;
                    _response.Result = jwtToken;
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
}