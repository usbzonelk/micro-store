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
    [Route("api/v1/products")]
    [ApiController]
    //[ApiVersion("2.0")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public ProductsController(ILogger<ProductsController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet(Name = "GetProducts")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetProducts()
        {
            IEnumerable<Product> allProducts = null;
            try
            {
                allProducts = await _unitOfWork.ProductRepository.GetAll();
                _response.Result = _mapper.Map<List<Product>>(allProducts);
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

        [HttpGet("{slug}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProduct(string slug)
        {

            Product chosenProduct;
            try
            {
                chosenProduct = await _unitOfWork.ProductRepository.Get(product => product.Slug == slug);
                _response.Result = _mapper.Map<ProductDTO>(chosenProduct);
                if (chosenProduct == null)
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

        [HttpGet("search", Name = "SearchProducts")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> SearchProducts([FromQuery] string query)
        {
            IEnumerable<Product> foundProducts;
            try
            {
                foundProducts = await _unitOfWork.ProductRepository.Search(query, "Title");
                if (foundProducts != null)
                {
                    _response.Result = _mapper.Map<List<ProductDTO>>(foundProducts);
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

        [HttpPost("create", Name = "CreateProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> CreateProduct([FromBody] ProductInputDTO newProductDTO)
        {

            try
            {
                if (newProductDTO == null)
                {
                    throw new Exception(message: "The data you entered is incorrect!");
                }

                var slugExists = await _unitOfWork.ProductRepository.Get(product => product.Slug == newProductDTO.Slug);
                if (slugExists != null)
                {
                    throw new Exception(message: "The slug you entred already exists");
                }
                else
                {
                    ProductType productTypeInput = await _unitOfWork.ProductTypesRepository.Get(productType => productType.ProductTypeID == newProductDTO.ProductTypeID);

                    if (productTypeInput == null)
                    {
                        throw new Exception(message: "The ProductTypeID you entred doesn't exist");

                    }

                    Product newProduct = _mapper.Map<Product>(newProductDTO);
                    newProduct.ProductType = productTypeInput;
                    await _unitOfWork.ProductRepository.Add(newProduct);
                    _response.Result = _mapper.Map<ProductDTO>(newProduct);
                    _response.Status = HttpStatusCode.Created;
                    return CreatedAtRoute("GetProduct", new { slug = newProduct.Slug }, _response);
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
        [HttpDelete("delete/{slug}", Name = "DeleteProduct")]
        public async Task<ActionResult<APIResponse>> DeleteProduct(string slug)
        {
            try
            {
                if (slug == null || slug == "")
                {
                    return BadRequest();
                }
                var productToBeDeleted = await _unitOfWork.ProductRepository.Get(product => product.Slug == slug);
                if (productToBeDeleted == null)
                {
                    return NotFound();
                }

                await _unitOfWork.ProductRepository.Remove(productToBeDeleted);
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

        [HttpPut("update/{slug}", Name = "Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateProduct(string slug, [FromBody] ProductUpdateDTO productUpdate)
        {
            try
            {
                if (productUpdate == null || slug == "")
                {
                    return BadRequest();
                }
                Product productToBeChanged = await _unitOfWork.ProductRepository.Get(product => product.Slug == slug);

                if (productToBeChanged == null)
                {
                    _response.Status = HttpStatusCode.NotFound;
                    _response.Successful = false;
                    _response.Errors = new List<string> { "The slug you've entered is incorrect" };
                    return NotFound(_response);
                }
                else
                {
                    _unitOfWork.ProductRepository.Detach(productToBeChanged);
                }
                ProductType productTypeInput = await _unitOfWork.ProductTypesRepository.Get(productType => productType.ProductTypeID == productUpdate.ProductTypeID);

                if (productTypeInput == null)
                {
                    _response.Status = HttpStatusCode.NotFound;
                    _response.Successful = false;
                    _response.Errors = new List<string> { "The ProductTypeID you entred doesn't exist" };
                    return NotFound(_response);

                }
                Product model = _mapper.Map<Product>(productUpdate);
                model.ProductType = productTypeInput;
                model.ProductID = productToBeChanged.ProductID;

                var updatedProduct = await _unitOfWork.ProductRepository.Update(model);
                _response.Status = HttpStatusCode.NoContent;
                _response.Successful = true;
                _response.Result = updatedProduct;
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

        [HttpPatch("updatePartially/{slug}", Name = "UpdatePartialProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialProduct(string slug, JsonPatchDocument<ProductDTO> productUpdate)
        {
            if (productUpdate == null || slug == null)
            {
                return BadRequest();
            }
            var product = await _unitOfWork.ProductRepository.Get(product => product.Slug == slug, tracked: false);

            ProductDTO proudctDTO = _mapper.Map<ProductDTO>(product);

            if (product == null)
            {
                _response.Status = HttpStatusCode.NotFound;
                _response.Successful = false;
                _response.Errors = new List<string> { "The slug you've entered is incorrect" };
                return NotFound(_response);
            }
            productUpdate.ApplyTo(proudctDTO);
            Product model = _mapper.Map<Product>(proudctDTO);

            await _unitOfWork.ProductRepository.Update(model);

            /* if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } */
            return NoContent();
        }

        [HttpGet("type/{typeName}", Name = "GetProductsOfType")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProductsOfType(string typeName)
        {

            ProductType chosenType;
            try
            {
                chosenType = await _unitOfWork.ProductTypesRepository.Get(ProductType => ProductType.TypeName == typeName, tracked: false);
                if (chosenType == null)
                {
                    throw new Exception("Entered product type doesn't exist");
                }
                else
                {
                    var productList = await _unitOfWork.ProductRepository.GetMany(product => product.ProductType == chosenType);
                    _response.Result = _mapper.Map<List<Product>>(productList);
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