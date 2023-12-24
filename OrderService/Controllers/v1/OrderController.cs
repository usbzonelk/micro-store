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


    }
}