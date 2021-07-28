using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostService.Data.AccountMock;
using PostService.Data.Post;
using PostService.Entities;
using PostService.Entities.Posts;
using PostService.Models.DTOs.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Controllers
{
    /// <summary>
    /// Post controller with endpoints for fetching posts
    /// </summary>
    [ApiController]
    [Route("api/posts")]
    [Produces("application/json", "application/xml")]
    public class PostController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IAccountMockRepository _accountMockRepository;
        private readonly IMapper _mapper;

        public PostController(IProductRepository productRepository, IServiceRepository serviceRepository, IAccountMockRepository accountMockRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _serviceRepository = serviceRepository;
            _accountMockRepository = accountMockRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns implemented options for API   
        /// </summary>
        /// <returns>Header key 'Allow' with allowed requests</returns>
        /// <remarks>
        /// Example of successful request \
        /// OPTIONS 'https://localhost:44377/api/posts' \
        /// </remarks>
        /// <response code="200">Return header key 'Allow' with allowed requests</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpOptions]
        public IActionResult GetPostOpstions()
        {
            Response.Headers.Add("Allow", "GET");
            return Ok();
        }

        /// <summary>
        /// Returns list of all posts 
        /// </summary>
        /// <param name="postType">Name of the post type (product or service)</param>
        /// <param name="username">Name of the user who posted</param>
        /// <returns>List of posts</returns>
        /// <response code="200">Returns list of posts</response>
        /// <response code="400">Bad request - e.g. wrong post type, wrong username</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">No posts found</response>
        /// <response code="500">Error on the server</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<PostDto>> GetPosts(string postType, string username)
        {
            try
            {
                if (string.IsNullOrEmpty(postType))
                {
                    List<Product> products;
                    List<Service> services;
                    if (!string.IsNullOrEmpty(username))
                    {
                        var account = _accountMockRepository.GetAccountByUsername(username);
                        if (account == null)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, "Account with given username does not exist");
                        }

                        products = _productRepository.GetProductsByAccountId(account.AccountId);
                        services = _serviceRepository.GetServicesByAccountId(account.AccountId);

                    }
                    else
                    {
                        products = _productRepository.GetProducts();
                        services = _serviceRepository.GetServices();
                    }
                    List<PostDto> posts = new List<PostDto>();
                    posts.AddRange(_mapper.Map<List<PostDto>>(products));
                    posts.AddRange(_mapper.Map<List<PostDto>>(services));
                    if (posts.Count == 0)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, "No posts found");
                    }
                    return Ok(posts);
                }
                else
                {
                    if (string.Equals(postType, "product"))
                    {
                        List<Product> products = _productRepository.GetProducts();
                        if (products == null || products.Count == 0)
                        {
                            return StatusCode(StatusCodes.Status404NotFound, "No products found");
                        }
                        return Ok(_mapper.Map<List<PostDto>>(products));
                    }
                    else if (string.Equals(postType, "service"))
                    {
                        List<Service> services = _serviceRepository.GetServices();
                        if (services == null || services.Count == 0)
                        {
                            return StatusCode(StatusCodes.Status404NotFound, "No services found");
                        }
                        return Ok(_mapper.Map<List<PostDto>>(services));
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "Wrong post type");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Returns post with postId
        /// </summary>
        /// <param name="postId">Post's Id</param>
        /// <returns> Post with postId</returns>
        ///<response code="200">Returns the post</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="403">Forbiden request - user with this role doesn't have permission to access endpoint</response>
        /// <response code="404">Post with this postId is not found</response>
        /// <response code="500">Error on the server</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin, User")]
        [HttpGet("{postId}")]
        public ActionResult<PostDto> GetPostById(Guid postId)
        {
            try
            {
                var product = _productRepository.GetProductById(postId);
                if (product != null)
                {
                    return Ok(_mapper.Map<PostDto>(product));
                }

                var service = _serviceRepository.GetServiceById(postId);
                if (service != null)
                {
                    return Ok(_mapper.Map<PostDto>(service));
                }
                return StatusCode(StatusCodes.Status404NotFound, "Post with this postId is not found");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
