using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostService.Data.PostHistories;
using PostService.Entities;
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
    public class PostHistoryController : ControllerBase
    {
        private readonly IPostHistoryRepository _postHistoryRepository;

        public PostHistoryController(IPostHistoryRepository postHistoryRepository)
        {
            _postHistoryRepository = postHistoryRepository;

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
        public IActionResult GetPostOpstions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }

        /// <summary>
        /// Returns list of all post histories
        /// </summary>
        /// <returns>List of post histories</returns>
        /// <response code="200">Returns list of post histories</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">No post histories found</response>
        /// <response code="500">Error on the server</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        ///<response code="200">Returns the post history</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">Post history with this postId is not found</response>
        /// <response code="500">Error on the server</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{postId}")]
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
    }
}
