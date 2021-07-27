using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ReactionService.Data.Reactions;
using ReactionService.Entities;
using ReactionService.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Controllers
{
    /// <summary>
    /// Reaction controller with endpoints for CRUD operations
    /// </summary>
    [ApiController]
    [Route("api/reactions")]
    [Produces("application/json", "application/xml")]
    [Authorize]
    public class ReactionController : ControllerBase
    {
        private readonly IReactionRepository _reactionRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        private readonly IFakeLogger _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public ReactionController(IReactionRepository reactionRepository, IMapper mapper, IFakeLogger logger,
                                    LinkGenerator linkGenerator, IHttpContextAccessor contextAccessor)
        {
            _reactionRepository = reactionRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Returns implemented options for API   
        /// </summary>
        /// <returns>Header key 'Allow' with allowed requests</returns>
        /// <remarks>
        /// Example of successful request \
        /// OPTIONS 'https://localhost:44389/api/reactions' \
        /// </remarks>
        /// <response code="200">Return header key 'Allow' with allowed requests</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpOptions]
        [AllowAnonymous]
        public IActionResult GetPostOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }

        /// <summary>
        /// Returns list of all reactions
        /// </summary>
        /// <returns>List of reactions</returns>
        /// <remarks>
        /// GET 'https://localhost:44389/api/reactions' \
        ///     Example of successful request \
        ///         --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJLZXkiOiJTZWNyZXRLZXlEdXNhbktyc3RpYzEyMyIsInJvbGUiOiJVc2VyIn0.4cCC6M5FbRuEDgB09F_9T-3To760pEx6ZXKEqrKsKxg' \
        /// </remarks>
        /// <response code="200">Returns list of reactions</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">No reactions found</response>
        /// <response code="500">There was an error on the server</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Reaction>> GetReactions()
        {
            try
            {
                var reactions = _reactionRepository.GetReactions();
                if (reactions == null || reactions.Count == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "No reactions found");
                }
                return Ok(reactions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Returns reaction with reactionId
        /// </summary>
        /// <param name="reactionId">Reaction's Id</param>
        /// <returns>Reaction with reactionId</returns>
        /// <remarks>
        /// Example of successful request \
        /// GET 'https://localhost:44389/api/reactions/19E0ACBF-5707-49EE-8CB6-134C00B7C10B' \
        ///     --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJLZXkiOiJTZWNyZXRLZXlEdXNhbktyc3RpYzEyMyIsInJvbGUiOiJVc2VyIn0.4cCC6M5FbRuEDgB09F_9T-3To760pEx6ZXKEqrKsKxg' \
        /// </remarks>
        /// <response code="200">Returns the reaction</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">Reaction with this reactionId is not found</response>
        /// <response code="500">There was an error on the server</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{reactionId}")]
        public ActionResult<Reaction> GetReactionById(Guid reactionId)
        {
            try
            {
                var reaction = _reactionRepository.GetReactionById(reactionId);
                if (reaction == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Reaction with this id is not found");
                }
                return Ok(reaction);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        /// <summary>
        /// Returns reaction(s) by reactionTypeId
        /// </summary>
        /// <param name="typeId">Reaction type's Id</param>
        /// <returns> Reaction(s) with reactionTypeId</returns>
        /// <remarks>        
        /// Example of a request to get reactions(s) by reactionTypeId
        /// GET 'https://localhost:44389/api/reactions/reactionType/2' \
         ///     --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJLZXkiOiJTZWNyZXRLZXlEdXNhbktyc3RpYzEyMyIsInJvbGUiOiJVc2VyIn0.4cCC6M5FbRuEDgB09F_9T-3To760pEx6ZXKEqrKsKxg' \
        /// </remarks>
        /// <response code="200">Returns the reaction(s)</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">Reaction with this reactionTypeId is not found</response>
        /// <response code="500">Error on the server</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("reactionTypeId/{typeId}")]
        public ActionResult<List<Reaction>> GetReactionByReactionTypeId(int typeId)
        {
            try
            {
                var reaction = _reactionRepository.GetReactionByReactionTypeId(typeId);
                if (reaction == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Reaction(s) with this id is not found");
                }
                return Ok(reaction);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        /// <summary>
        /// Returns reaction(s) by postId
        /// </summary>
        /// <param name="postId">Post's Id</param>
        /// <returns> Reaction(s) with postId</returns>
        /// <remarks>        
        /// Example of a request to get reactions(s) by postId
        /// GET 'https://localhost:44389/api/reactions/postId/8ccb1467-9f38-4164-88da-15882fe82e58' \
        ///     --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJLZXkiOiJTZWNyZXRLZXlEdXNhbktyc3RpYzEyMyIsInJvbGUiOiJVc2VyIn0.4cCC6M5FbRuEDgB09F_9T-3To760pEx6ZXKEqrKsKxg' \
        /// </remarks>
        /// <response code="200">Returns the reaction(s)</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">Reaction with this postId is not found</response>
        /// <response code="500">Error on the server</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("postId/{postId}")]
        public ActionResult<List<Reaction>> GetReactionByPostId(Guid postId)
        {
            try
            {
                var reaction = _reactionRepository.GetReactionByPostId(postId);
                if (reaction == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Reaction(s) with this id is not found");
                }
                return Ok(reaction);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }
    }
}
