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
    [Route("api/v1/public/login")]
    [ApiController]
    //[ApiVersion("2.0")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        protected APIOutDTO _response;
        private readonly IMapper _mapper;
        private IUserService _userService;
        private IAdminService _adminService;
        public AuthController(ILogger<AuthController> logger, IMapper mapper, IUserService userService, IAdminService adminService)
        {
            _logger = logger;
            _mapper = mapper;
            _response = new();
            _userService = userService;
            _adminService = adminService;
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

    [Route("api/v1/public/reg")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        protected APIOutDTO _response;
        private IUserService _userService;
        private IAdminService _adminService;
        public RegistrationController(ILogger<AuthController> logger, IUserService userService, IAdminService adminService)
        {
            _logger = logger;
            _response = new();
            _userService = userService;
            _adminService = adminService;
        }

        [HttpPost("user", Name = "ResgisterUser")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIOutDTO>> ResgisterUser([FromBody] UserLoginDTO userInfo)
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
                UserLoginDTO userFound = await _userService.CreateNewUser(userInfo);

                if (userFound == null)
                {
                    _response.Status = HttpStatusCode.NotFound;
                    _response.Result = userLoginOutput;
                    _response.Successful = false;

                    return NotFound(_response);
                }
                else
                {
                    _response.Status = HttpStatusCode.OK;
                    _response.Result = true;
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

        [HttpGet("verify/user", Name = "VarifyUser")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIOutDTO>> VarifyUser([FromQuery] VerifyUserDTO verifyInfo)
        {
            try
            {
                if ((verifyInfo.Email is null) || (verifyInfo.UserToken is null))
                {
                    _response.Status = HttpStatusCode.NotFound;
                    _response.Result = false;
                    _response.Successful = false;

                    return NotFound(_response);
                }
                bool userVerification = await _userService.VerifyUser(verifyInfo);

                if (userVerification == null || userVerification is false)
                {
                    _response.Status = HttpStatusCode.BadRequest;
                    _response.Result = false;
                    _response.Successful = false;

                    return NotFound(_response);
                }
                else
                {
                    _response.Status = HttpStatusCode.OK;
                    _response.Result = true;
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


        [HttpPost("admin", Name = "RegisterAdmin")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIOutDTO>> RegisterAdmin([FromBody] AdminSignupDTO adminInfo)
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
                var adminFound = await _adminService.CreateNewAdmin(adminInfo);

                if (adminFound is null)
                {
                    _response.Status = HttpStatusCode.NotFound;
                    _response.Result = userLoginOutput;
                    _response.Successful = false;

                    return NotFound(_response);
                }
                else
                {
                    _response.Status = HttpStatusCode.OK;
                    _response.Result = true;
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
    [Route("api/v1/public/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        protected APIOutDTO _response;
        private IProductService _productService;
        public ProductsController(ILogger<AuthController> logger, IProductService productService)
        {
            _logger = logger;
            _response = new();
            _productService = productService;
        }

        [HttpGet("getAll", Name = "GetAllProducts")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIOutDTO>> GetAllProducts()
        {
            try
            {
                var allProducts = await _productService.GetProducts();

                if (allProducts is null || allProducts.IsSuccessful is false)
                {
                    _response.Status = HttpStatusCode.NotFound;
                    _response.Result = null;
                    _response.Successful = false;

                    return NotFound(_response);
                }
                else
                {
                    _response.Status = HttpStatusCode.OK;
                    _response.Result = allProducts;
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

        [HttpGet("product/{slug}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIOutDTO>> GetProduct(string slug)
        {
            try
            {
                if (slug is null)
                {
                    _response.Status = HttpStatusCode.NotFound;
                    _response.Result = null;
                    _response.Successful = false;

                    return BadRequest(_response);
                }
                var productFound = await _productService.GetProduct(slug);

                if (productFound is null || productFound.IsSuccessful is false || productFound.Output is null)
                {
                    _response.Status = HttpStatusCode.BadRequest;
                    _response.Result = null;
                    _response.Successful = false;

                    return NotFound(_response);
                }
                else
                {
                    _response.Status = HttpStatusCode.OK;
                    _response.Result = productFound;
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


        [HttpGet("search/{query}", Name = "SearchProducts")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIOutDTO>> SearchProducts(string query)
        {
            try
            {
                if (query is null)
                {
                    _response.Status = HttpStatusCode.NotFound;
                    _response.Result = null;
                    _response.Successful = false;

                    return NotFound(_response);
                }
                var adminFound = await _productService.SearchProducts(query);

                _response.Status = HttpStatusCode.OK;
                _response.Result = adminFound;

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