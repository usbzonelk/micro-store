using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AdminService.Models;
using AdminService.Models.DTO;
using AdminService.Repository;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using AdminService.Utils;

using System.Text.Json;

namespace AdminService.Controllers.v1
{
    [Route("api/v1/admins")]
    [ApiController]
    //[ApiVersion("2.0")]
    public class AdminsController : ControllerBase
    {
        private readonly ILogger<AdminsController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public AdminsController(ILogger<AdminsController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet(Name = "GetAdmins")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAdmins()
        {
            IEnumerable<Admin> allAdmins = null;
            try
            {
                allAdmins = await _unitOfWork.AdminRepository.GetAll();
                _response.Result = _mapper.Map<List<Admin>>(allAdmins);
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

        [HttpGet("{email}", Name = "GetAdmin")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAdmin(string email)
        {

            Admin chosenAdmin;
            try
            {
                chosenAdmin = await _unitOfWork.AdminRepository.Get(admin => admin.Email == email);
                _response.Result = _mapper.Map<AdminDTO>(chosenAdmin);
                if (chosenAdmin == null)
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

        [HttpPost("create", Name = "CreateAdmin")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> CreateAdmin([FromBody] AdminInputDTO newAdminDTO)
        {

            try
            {
                if (newAdminDTO == null)
                {
                    throw new Exception(message: "The data you entered is incorrect!");
                }

                var adminExists = await _unitOfWork.AdminRepository.Get(admin => admin.Email == newAdminDTO.Email);
                if (adminExists != null)
                {
                    throw new Exception(message: "The email you entred is already registered");
                }
                else
                {
                    Admin newAdmin = _mapper.Map<Admin>(newAdminDTO);
                    newAdmin.Password = HashText.HashPass(newAdminDTO.Password);

                    await _unitOfWork.AdminRepository.Add(newAdmin);
                    _response.Result = _mapper.Map<AdminDTO>(newAdmin);
                    _response.Status = HttpStatusCode.Created;
                    return CreatedAtRoute("GetAdmin", new { email = newAdmin.Email }, _response);
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

        [HttpPatch("updatepassword/", Name = "UpdateAdminPassword")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAdminPassword([FromBody] AdminPasswordUpdateDTO updateInfo)
        {
            if (updateInfo == null || updateInfo.Email == null || updateInfo.Password == null || updateInfo.OldPassword == null)
            {
                return BadRequest();
            }
            var admin = await _unitOfWork.AdminRepository.Get(u => u.Email == updateInfo.Email, tracked: false);

            if (updateInfo.Password == updateInfo.OldPassword)
            {
                _response.Status = HttpStatusCode.NotFound;
                _response.Successful = false;
                _response.Errors = new List<string> { "Passwords can't be the same" };
                return NotFound(_response);
            }

            if (admin == null)
            {
                _response.Status = HttpStatusCode.NotFound;
                _response.Successful = false;
                _response.Errors = new List<string> { "The email you've entered is incorrect" };
                return NotFound(_response);
            }
            if (admin.IsActive == false)
            {
                _response.Status = HttpStatusCode.Forbidden;
                _response.Successful = false;
                _response.Errors = new List<string> { "The account is not active!" };
                return NotFound(_response);
            }
            else
            {
                bool isOldPassCorrect = HashText.VerifyPass(updateInfo.OldPassword, admin.Password);
                if (!isOldPassCorrect)
                {
                    _response.Status = HttpStatusCode.Forbidden;
                    _response.Successful = false;
                    _response.Result = new List<string> { "The old password is incorrect!" };
                    return NotFound(_response);
                }
                admin.Password = HashText.HashPass(updateInfo.Password);
                var result = await _unitOfWork.AdminRepository.Update(admin);

                _response.Status = HttpStatusCode.OK;
                _response.Successful = true;
                _response.Result = $"You've successfully updated password for: {updateInfo.Email}";
                return Ok(_response);

            }


        }


        [HttpPatch("togglestatus/{email}", Name = "ToggleAdminStatus")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ToggleAdminStatus(string email)
        {
            if (email is null)
            {
                return BadRequest();
            }
            var admin = await _unitOfWork.AdminRepository.Get(u => u.Email == email, tracked: false);

            if (admin is null)
            {
                _response.Status = HttpStatusCode.NotFound;
                _response.Successful = false;
                _response.Errors = new List<string> { "The email you've entered is incorrect" };
                return NotFound(_response);
            }
            else
            {
                admin.IsActive = !admin.IsActive;
                var result = await _unitOfWork.AdminRepository.Update(admin);

                _response.Status = HttpStatusCode.OK;
                _response.Successful = true;
                _response.Result = $"You've successfully updated account status";
                return Ok(_response);
            }
        }





        [HttpPost("authorize", Name = "AuthAdmin")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> AuthAdmin([FromBody] AdminInputDTO adminLogins)
        {

            try
            {
                if (adminLogins == null)
                {
                    throw new Exception(message: "The data you entered is incorrect!");
                }

                var adminExists = await _unitOfWork.AdminRepository.Get(admin => admin.Email == adminLogins.Email);
                if (adminExists == null)
                {
                    throw new Exception(message: "The email you entred is not registered");
                }
                else
                {
                    bool passVerify = HashText.VerifyPass(adminLogins.Password, adminExists.Password);
                    if (!passVerify)
                    {
                        _response.Result = "Incorrect password!";
                        _response.Status = HttpStatusCode.Forbidden;
                        return BadRequest(_response);
                    }
                    else
                    {
                        _response.Result = "Admin authenticated successfully!";
                        _response.Status = HttpStatusCode.OK;
                        return Ok(_response);

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