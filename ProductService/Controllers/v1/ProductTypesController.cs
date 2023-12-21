using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;
using ProductService.Models.DTO;
using ProductService.Repository;
using System.Net;
using System.Text.Json;

namespace ProductService.Controllers.v1
{
    [Route("api/v1/producttypes")]
    [ApiController]
    //[ApiVersion("2.0")]
    public class ProductTypesController : ControllerBase
    {
        private readonly ILogger<ProductTypesController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public ProductTypesController(ILogger<ProductTypesController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet(Name = "GetProductTypes")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetProductTypes()
        {
            IEnumerable<ProductType> allProductTypes = null;
            try
            {
                allProductTypes = await _unitOfWork.ProductTypesRepository.GetAll();
                _response.Result = _mapper.Map<List<ProductType>>(allProductTypes);
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

        [HttpPost("create", Name = "CreateProductType")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> CreateProductType([FromBody] ProductTypeInputDTO newProductTypeDTO)
        {

            try
            {
                if (newProductTypeDTO == null)
                {
                    throw new Exception(message: "The data you entered is incorrect!");
                }

                var slugExists = await _unitOfWork.ProductTypesRepository.Get(productType => productType.TypeName == newProductTypeDTO.TypeName);
                if (slugExists != null)
                {
                    throw new Exception(message: "The product type you entred already exists");
                }
                else
                {

                    ProductType newProductType = _mapper.Map<ProductType>(newProductTypeDTO);
                    await _unitOfWork.ProductTypesRepository.Add(newProductType);
                    _response.Result = _mapper.Map<ProductTypeDTO>(newProductType);
                    _response.Status = HttpStatusCode.Created;
                    return CreatedAtRoute("CreateProductType", new { typeName = newProductType.TypeName }, _response);
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
        [HttpDelete("delete/{productTypeName}", Name = "DeleteProductType")]
        public async Task<ActionResult<APIResponse>> DeleteProductType(string productTypeName)
        {
            try
            {
                if (productTypeName == null || productTypeName == "")
                {
                    return BadRequest();
                }
                var productTypeToBeDeleted = await _unitOfWork.ProductTypesRepository.Get(productType => productType.TypeName == productTypeName);
                if (productTypeToBeDeleted == null)
                {
                    return NotFound();
                }

                await _unitOfWork.ProductTypesRepository.Remove(productTypeToBeDeleted);
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

        [HttpPut("updatetype/{productTypeName}", Name = "UpdateProductType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateProductType(string productTypeName, [FromBody] ProductTypeInputDTO productTypeUpdate)
        {
            Console.WriteLine(productTypeName);
            try
            {
                if (productTypeUpdate == null || productTypeName == "")
                {
                    return BadRequest();
                }
                ProductType productTypeToBeChanged = await _unitOfWork.ProductTypesRepository.Get(productType => productType.TypeName == productTypeName);

                if (productTypeToBeChanged == null)
                {
                    _response.Status = HttpStatusCode.NotFound;
                    _response.Successful = false;
                    _response.Errors = new List<string> { "The product type you've entered is incorrect" };
                    return NotFound(_response);
                }
                else
                {
                    _unitOfWork.ProductTypesRepository.Detach(productTypeToBeChanged);
                }

                ProductType model = _mapper.Map<ProductType>(productTypeUpdate);
                model.ProductTypeID = productTypeToBeChanged.ProductTypeID;
                var updatedProductType = await _unitOfWork.ProductTypesRepository.Update(model);
                _response.Status = HttpStatusCode.NoContent;
                _response.Successful = true;
                _response.Result = updatedProductType;
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

    }

}