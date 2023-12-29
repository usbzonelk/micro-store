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

        
    }
}