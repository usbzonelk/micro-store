using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UserService.Models;
using UserService.Models.DTO;
using UserService.Repository;
using System.Net;
using System.Text.Json;

namespace UserService.Controllers.v1
{
    [Route("api/v1/users")]
    [ApiController]
    //[ApiVersion("2.0")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public UsersController(ILogger<UsersController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet(Name = "GetUsers")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetUsers()
        {
            IEnumerable<User> allUsers = null;
            try
            {
                allUsers = await _unitOfWork.UserRepository.GetAll();
                _response.Result = _mapper.Map<List<User>>(allUsers);
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

        [HttpGet("{email}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUser(string email)
        {

            User chosenUser;
            try
            {
                chosenUser = await _unitOfWork.UserRepository.Get(user => user.Email == email);
                _response.Result = _mapper.Map<UserDTO>(chosenUser);
                if (chosenUser == null)
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

        [HttpGet("search", Name = "SearchUsers")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> SearchUsers([FromQuery] string query)
        {
            IEnumerable<User> foundUsers;
            try
            {
                foundUsers = await _unitOfWork.UserRepository.Search(query, "FirstName");
                if (foundUsers != null)
                {
                    _response.Result = _mapper.Map<List<UserDTO>>(foundUsers);
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

        [HttpPost("create", Name = "CreateUser")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> CreateUser([FromBody] UserCreateDTO newUserDTO)
        {

            try
            {
                if (newUserDTO == null)
                {
                    throw new Exception(message: "The data you entered is incorrect!");
                }

                var userExists = await _unitOfWork.UserRepository.Get(user => user.Email == newUserDTO.Email);
                if (userExists != null)
                {
                    throw new Exception(message: "The email you entred is already registered");
                }
                else
                {
                    User newUser = _mapper.Map<User>(newUserDTO);
                    await _unitOfWork.UserRepository.Add(newUser);
                    _response.Result = _mapper.Map<UserDTO>(newUser);
                    _response.Status = HttpStatusCode.Created;
                    return CreatedAtRoute("GetUser", new { email = newUser.Email }, _response);
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


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("delete/{email}", Name = "DeleteUser")]
        public async Task<ActionResult<APIResponse>> DeleteUser(string email)
        {
            try
            {
                if (email == null || email == "")
                {
                    return BadRequest();
                }
                var userToBeDeleted = await _unitOfWork.UserRepository.Get(u => u.Email == email);
                if (userToBeDeleted == null)
                {
                    return NotFound();
                }

                await _unitOfWork.UserRepository.Remove(userToBeDeleted);
                _response.Status = HttpStatusCode.NoContent;
                _response.Successful = true;
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
        /*
                [HttpPatch("updatePartially/{slug}", Name = "UpdatePartialUser")]
                [ProducesResponseType(StatusCodes.Status204NoContent)]
                [ProducesResponseType(StatusCodes.Status400BadRequest)]
                public async Task<IActionResult> UpdatePartialUser(string slug, JsonPatchDocument<UserDTO> productUpdate)
                {
                    if (productUpdate == null || slug == null)
                    {
                        return BadRequest();
                    }
                    var product = await _unitOfWork.UserRepository.Get(product => product.Slug == slug, tracked: false);

                    UserDTO proudctDTO = _mapper.Map<UserDTO>(product);

                    if (product == null)
                    {
                        _response.Status = HttpStatusCode.NotFound;
                        _response.Successful = false;
                        _response.Errors = new List<string> { "The slug you've entered is incorrect" };
                        return NotFound(_response);
                    }
                    productUpdate.ApplyTo(proudctDTO);
                    User model = _mapper.Map<User>(proudctDTO);

                    await _unitOfWork.UserRepository.Update(model);

                    // if (!ModelState.IsValid)
                    //  {
                    //       return BadRequest(ModelState);
                    //  } 
                    //   return NoContent();
                }
                /*
                            [HttpGet("type/{typeName}", Name = "GetUsersOfType")]
                            [ProducesResponseType(StatusCodes.Status403Forbidden)]
                            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
                            [ProducesResponseType(StatusCodes.Status200OK)]
                            [ProducesResponseType(StatusCodes.Status404NotFound)]
                            public async Task<ActionResult<APIResponse>> GetUsersOfType(string typeName)
                            {

                                UserType chosenType;
                                try
                                {
                                    chosenType = await _unitOfWork.UserTypesRepository.Get(UserType => UserType.TypeName == typeName, tracked: false);
                                    if (chosenType == null)
                                    {
                                        throw new Exception("Entered product type doesn't exist");
                                    }
                                    else
                                    {
                                        var productList = await _unitOfWork.UserRepository.GetMany(product => product.UserType == chosenType);
                                        _response.Result = _mapper.Map<List<User>>(productList);
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
                                */

    }

}