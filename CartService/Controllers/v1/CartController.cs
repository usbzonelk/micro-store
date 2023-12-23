using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CartService.Models;
using CartService.Models.DTO;
using CartService.Repository;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;

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

        public CartsController(ILogger<CartsController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
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
                int userID = 0; // todo: verify UiD
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


    }
}


/*

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
                    newUser.Password = HashText.HashPass(newUserDTO.Password);

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




*/