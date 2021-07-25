using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using PostService.Data.Post;
using PostService.Data.PostHistories;
using PostService.Entities;
using PostService.Entities.Posts;
using PostService.Exceptions;
using PostService.Logger;
using PostService.Models.DTOs.Post;
using PostService.Models.DTOs.PostHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Controllers
{
    /// <summary>
    /// Product controller with endpoints for CRUD operations
    /// </summary>
    [ApiController]
    [Route("api/products")]
    [Produces("application/json", "application/xml")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        private readonly IFakeLogger _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPostHistoryRepository _postHistoryRepository;

        public ProductController(IProductRepository productRepository, IMapper mapper, IFakeLogger logger, 
                                    LinkGenerator linkGenerator, IHttpContextAccessor contextAccessor, IPostHistoryRepository postHistoryRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _postHistoryRepository = postHistoryRepository;
        }

        /// <summary>
        /// Returns implemented options for API   
        /// </summary>
        /// <returns>Header key 'Allow' with allowed requests</returns>
        /// <remarks>
        /// Example of successful request \
        /// OPTIONS 'https://localhost:44377/api/products' \
        /// </remarks>
        /// <response code="200">Return header key 'Allow' with allowed requests</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpOptions]
        [AllowAnonymous]
        public IActionResult GetPostOpstions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }

        /// <summary>
        /// Returns list of all products
        /// </summary>
        /// <param name="productName">Name of the product</param>
        /// <returns>List of products</returns>
        /// <response code="200">Returns list of products</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">No products found</response>
        /// <response code="500">There was an error on the server</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public ActionResult<List<Product>> GetProducts(string productName)
        {
            try
            {
                var products = _productRepository.GetProducts(productName);
                if (products == null || products.Count == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "No products found");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Returns product with productId
        /// </summary>
        /// <param name="productId">Products's Id</param>
        /// <returns>Product with productId</returns>
        /// <response code="200">Returns the product</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">Product with this productId is not found</response>
        /// <response code="500">There was an error on the server</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{productId}")]
        public ActionResult<Product> GetProductById(Guid productId)
        {
            try
            {
                var product = _productRepository.GetProductById(productId);
                if (product == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Product with this id is not found");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        /// <summary>
        /// Creates product
        /// </summary>
        /// <param name="productDto">Model of product to create</param>
        /// <remarks>
        /// POST 'https://localhost:44377/api/products/' \
        /// Example of a request to create product \
        ///  --header 'Authorization: TODO - dodati jwt' \
        /// {
        /// "weight": "130g",
        /// "postName": "Nike sportska majica",
        /// "postImage": "https://www.intersport.rs/media/catalog/product/cache/382907d7f48ae2519bf16cd5f39b77f9/a/r/ar5004-010-phsfh001-1000.jpg",
        /// "description": "Nike Sportswear T-Shirt je muška majica, koju odlikuje udobnost, jer je napravljena od mekog materijala.",
        /// "price": 2310,
        /// "currency": "Rsd",
        /// "category": "Sportska odeća",
        /// "accountId": "59ed7d80-39c9-42b8-a822-70ddd295914a"
        ///  }
        /// </remarks>
        /// <response code="201">Returns the created product</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="500">There was an error on the server</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public ActionResult<ProductConfirmationDto> CreateProduct([FromBody] ProductCreationDto productDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDto);
                product.PublicationDate = DateTime.Now;
                _productRepository.CreateProduct(product);
                _productRepository.SaveChanges();
                _logger.Log(LogLevel.Information, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully created new product with ID {0} in database", product.PostId), null);
                string location = _linkGenerator.GetPathByAction("GetProductById", "Product", new { productId = product.PostId });
                return Created(location, _mapper.Map<ProductConfirmationDto>(product));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while creating product, message: {0}", ex.Message), null);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates product
        /// </summary>
        /// <param name="productUpdateDto">Model of product for udpdate</param>
        /// <param name="productId">Product id</param>
        /// <param name="userId">ID of the user who sends the request/param>
        /// <returns>Confirmation of update</returns>
        /// <remarks>
        /// POST 'https://localhost:44377/api/products/' \
        /// Example of a request to update product \
        ///  --header 'Authorization: TODO - dodati jwt' \
        ///  --param  'productId = example of Guid'
        ///  --param  correct userId 'userId = 59ed7d80-39c9-42b8-a822-70ddd295914a'
        ///           wrong userId 'userId = 42b70088-9dbd-4b19-8fc7-16414e94a8a6'
        /// {
        /// "postName": "Nike majica",
        /// "postImage": "https://www.intersport.rs/media/catalog/product/cache/382907d7f48ae2519bf16cd5f39b77f9/a/r/ar5004-010-phsfh001-1000.jpg",
        /// "description": "Nike Sportswear T-Shirt je muška majica, koja je jako udobna jer je napravljena od mekog materijala.",
        /// "price": 2450,
        /// "currency": "Rsd",
        /// "category": "Sportska odeća",
        /// "accountId": "59ed7d80-39c9-42b8-a822-70ddd295914a",
        /// "weight": "110g"
        /// }
        /// </remarks>
        /// <response code="200">Returns updated product</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="403">Forbiden request - user with this userId doesn't have permission to update product</response>
        /// <response code="404">Product with productId is not found</response>
        /// <response code="409">Conflict - Foreign key constraint violation</response>
        /// <response code="500">Error on the server while updating</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{productId}")]
        public ActionResult<Product> UpdateProduct([FromBody] ProductUpdateDto productUpdateDto, Guid productId, [FromHeader] Guid userId)
        {
            try
            {
                if (productUpdateDto.AccountId != userId)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, String.Format("User whit ID {0} doesn't have permission to update this product because he isn't product owner", userId));
                }

                var oldProduct = _productRepository.GetProductById(productId);
                if (oldProduct == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, String.Format("There is no product with ID {0}", productId));
                }
                Product newProduct = _mapper.Map<Product>(productUpdateDto);

                _productRepository.UpdateProduct(oldProduct, newProduct);
                _productRepository.SaveChanges();
                _logger.Log(LogLevel.Information, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully updated product with ID {0}", productId), null);

                return Ok(oldProduct);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while updating product with ID {0}, message: {1}", productId, ex.Message), null);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Deleting product with productId
        /// </summary>
        /// <param name="productId">Product's Id</param>
        /// <param name="userId">ID of the user who sends the request/param>
        /// <returns>Status 204 - NoContent</returns>
        /// <remarks>        
        /// Example of a request to delete product
        /// DELETE 'https://localhost:44377/api/products' \
        ///   --param  'productId = 23d2cce9-86d7-4bff-887e-f7712b16766d'
        ///   --param  userId 'userId = 42b70088-9dbd-4b19-8fc7-16414e94a8a6'
        ///           wrong userId 'userId = 59ed7d80-39c9-42b8-a822-70ddd295914a'
        /// </remarks>
        /// <response code="204">Prdouct successfully deleted</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="403">Forbiden request - user with this userId doesn't have permission to delete product</response>
        /// <response code="404">Product with this productId is not found</response>
        /// <response code="409">Product reference in another table</response>
        /// <response code="500">There was an error on the server</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{productId}")]
        public IActionResult DeleteProduct(Guid productId,[FromHeader] Guid userId)
        {
            try
            {
                var product = _productRepository.GetProductById(productId);

                if (product.AccountId != userId)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, String.Format("User whit ID {0} doesn't have permission to delete this product because he isn't product owner", userId));
                }

                if (product == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, String.Format("There is no product with ID {0}!", productId));
                }

                List<PostHistory> postHistories = _postHistoryRepository.GetPostHistoryByPostId(productId);
                if (postHistories.Count > 0)
                {
                    throw new ReferentialConstraintException("Value of product is referenced in another table");
                }
                _productRepository.DeleteProduct(productId);
                _productRepository.SaveChanges();

                return NoContent();
            }

            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while deleting product with ID {0}, message: {1}", productId, ex.Message), null);
                if (ex.GetBaseException().GetType() == typeof(ReferentialConstraintException))
                {
                    return StatusCode(StatusCodes.Status409Conflict, ex.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting product!");
            }
        }
    }
}
