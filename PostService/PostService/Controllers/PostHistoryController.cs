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
using PostService.Models.DTOs.PostHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Controllers
{
    /// <summary>
    /// PostHistory controller with endpoints for CRUD operations
    /// </summary>
    [ApiController]
    [Route("api/postHistories")]
    [Produces("application/json", "application/xml")]
    [Authorize]
    public class PostHistoryController : ControllerBase
    {
        private readonly IPostHistoryRepository _postHistoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        private readonly IFakeLogger _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public PostHistoryController(IPostHistoryRepository postHistoryRepository, IMapper mapper, LinkGenerator linkGenerator,
                                     IProductRepository productRepository, IServiceRepository serviceRepository, IFakeLogger logger, IHttpContextAccessor contextAccessor)
        {
            _postHistoryRepository = postHistoryRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _productRepository = productRepository;
            _serviceRepository = serviceRepository;
        }

        /// <summary>
        /// Returns implemented options for API   
        /// </summary>
        /// <returns>Header key 'Allow' with allowed requests</returns>
        /// <remarks>
        /// Example of successful request \
        /// OPTIONS 'https://localhost:44377/api/postHistories' \
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
        /// Returns list of all post histories
        /// </summary>
        /// <returns>List of post histories</returns>
        /// <remarks>        
        /// Example of a request to get all post histories
        /// GET 'https://localhost:44377/api/postHistories' 
        /// </remarks>
        /// <response code="200">Returns list of post histories</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">No post histories found</response>
        /// <response code="500">Error on the server</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public ActionResult<List<PostHistory>> GetPostHisttories()
        {
            try
            {
                var postHistories = _postHistoryRepository.GetPostHistories();
                if (postHistories == null || postHistories.Count == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "No post histories found");
                }
                return Ok(postHistories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Returns post history by postId
        /// </summary>
        /// <param name="postId">Post's Id</param>
        /// <returns> Post history with postId</returns>
        /// <remarks>        
        /// Example of a request to get postHistory by postId
        /// GET 'https://localhost:44377/api/postHistories/' \
        ///     --param  'postId = 23d2cce9-86d7-4bff-887e-f7712b16766d'
        /// </remarks>
        /// <response code="200">Returns the post history</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">Post history with this postId is not found</response>
        /// <response code="500">Error on the server</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{postId}")]
        [AllowAnonymous]
        public ActionResult<List<PostHistory>> GetPostHistoryByPostId(Guid postId)
        {
            try
            {
                var postHistory = _postHistoryRepository.GetPostHistoryByPostId(postId);
                if (postHistory == null || postHistory.Count == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Post history with this postId is not found");
                }
                return Ok(postHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        /// <summary>
        /// Creates post history for a specific post
        /// </summary>
        /// <param name="postHistoryDto">Model of post history</param>
        /// <remarks>
        /// POST 'https://localhost:44377/api/postHistories/' \
        /// Example of a request to create postHistory \
        ///  --header 'Authorization: TODO - dodati jwt' \
        /// {     \
        ///  "Price": 600, \
        ///  "DateTo": "2021-11-15T09:00:00"\
        ///  "PostId": "54f9baf6-271e-40cb-8d80-a27980fc8b63"
        /// } 
        /// </remarks>
        /// <response code="201">Returns the created post history</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="500">There was an error on the server</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public ActionResult<PostHistory> CreatePostHistory([FromBody] PostHistoryCreationDto postHistoryDto)
        {
            try
            {
                PostHistory postHistory = _mapper.Map<PostHistory>(postHistoryDto);
                postHistory.DateFrom = DateTime.Now;
                _postHistoryRepository.CreatePostHistory(postHistory);
                _postHistoryRepository.SaveChanges();
                _logger.Log(LogLevel.Information, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully created new post history with ID {0} in database", postHistory.PostId), null);
                var location = _linkGenerator.GetPathByAction("GetPostHistoryByPostId", "PostHistory", new { postId = postHistory.PostId });
                return Created(location, postHistory);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while creating postHistory, message: {0}", ex.Message), null);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates post history
        /// </summary>
        /// <param name="postHistoryUpdateDto">Model of post history for udpdate</param>
        /// <param name="postHistoryId">Post history id</param>
        /// <returns>Confirmation of update</returns>
        /// <remarks>
        /// POST 'https://localhost:44377/api/postHistories/' \
        /// Example of a request to create postHistory \
        ///  --header 'Authorization: TODO - dodati jwt' \
        ///  --param  'postHistoryId = 9'
        /// {     \
        ///  "Price": 550, \
        ///  "DateFrom": "2020-10-15T09:00:00"\
        ///  "DateTo": "2021-11-15T09:00:00"\
        ///  "PostId": "54f9baf6-271e-40cb-8d80-a27980fc8b63"
        /// } 
        /// </remarks>
        /// <response code="200">Returns updated post history</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">Post history with postHistoryId is not found</response>
        ///<response code="422">Foreign key constraint violation</response>
        /// <response code="500">Error on the server while updating</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{postHistoryId}")]
        public ActionResult<PostHistory> UpdatePostHistory([FromBody] PostHistoryUpdateDto postHistoryUpdateDto, int postHistoryId)
        {
            try
            {
                var oldPostHistory = _postHistoryRepository.GetPostHistoryById(postHistoryId);
                if (oldPostHistory == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "There is no post history with that ID");
                }
                PostHistory newPostHistory = _mapper.Map<PostHistory>(postHistoryUpdateDto);

                Product product = _productRepository.GetProductById(newPostHistory.PostId);
                Service service = _serviceRepository.GetServiceById(newPostHistory.PostId);
                if (product == null && service == null)
                {
                    throw new ForeignKeyConstraintException("Foreign key constraint violated. Post with that ID doesn't exist!");
                }

                _postHistoryRepository.UpdatePostHistory(oldPostHistory, newPostHistory);
                _postHistoryRepository.SaveChanges();
                _logger.Log(LogLevel.Information, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully updated post history with ID {0}", postHistoryId), null);

                return Ok(oldPostHistory);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while updating post history with ID {0}, message: {1}", postHistoryId, ex.Message), null);
                if (ex.GetType().IsAssignableFrom(typeof(ForeignKeyConstraintException)))
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// Deleting post history with postHistoryId
        /// </summary>
        /// <param name="postHistoryId">PostHistory's Id</param>
        /// <returns>Status 204 - NoContent</returns>
        /// <remarks>        
        /// Example of a request to delete postHistory
        /// DELETE 'https://localhost:44377/api/postHistories' \
        ///   --param  'postHistoryId = 7'
        /// </remarks>
        /// <response code="204">PostHistory successfully deleted</response>
        /// <response code="401" >Unauthorized user</response>
        /// <response code="404">Post history with this postHistoryId is not found</response>
        /// <response code="500">There was an error on the server</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{postHistoryId}")]
        public IActionResult DeletePostHistory(int postHistoryId)
        {
            try
            {
                var postHistory = _postHistoryRepository.GetPostHistoryById(postHistoryId);
                if (postHistory == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "There is no post history with that id!");
                }
                _postHistoryRepository.DeletePostHistory(postHistoryId);
                _postHistoryRepository.SaveChanges();

                return NoContent();
            }

            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while deleting postHistory with ID {0}, message: {1}", postHistoryId, ex.Message), null);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting comment!");
            }
        }
    }
}
