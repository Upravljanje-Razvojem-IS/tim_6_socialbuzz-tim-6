using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostService.Data.Post;
using PostService.Entities.Posts;
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
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
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
        ///<response code="200">Returns the product</response>
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

    }
}
