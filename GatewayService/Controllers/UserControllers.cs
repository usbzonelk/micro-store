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
        private IAdminService _adminService;
        public UserManageController(ILogger<AuthController> logger, IUserService userService, IAdminService adminService)
        {
            _logger = logger;
            _response = new();
            _userService = userService;
            _adminService = adminService;
        }

        [HttpPost("user", Name = "LogInUser2")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIOutDTO>> LogInUser2(UserLoginDTO userInfo)
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
        [HttpPost("admin", Name = "LogInAdmin2")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIOutDTO>> LogInAdmin2(AdminSignupDTO adminInfo)
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