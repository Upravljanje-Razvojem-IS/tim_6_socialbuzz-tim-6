using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using ReactionService.Data.BlockingMock;
using ReactionService.Data.FollowingMock;
using ReactionService.Data.Reactions;
using ReactionService.Data.ReactionTypes;
using ReactionService.Entities;
using ReactionService.Exceptions;
using ReactionService.Logger;
using ReactionService.Models.DTOs.Reactions;
using ReactionService.Models.Mocks;
using ReactionService.ServiceCalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        private readonly IReactionTypeRepository _reactionTypeRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        private readonly IFakeLogger _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPostService _postService;

        public ReactionController(IReactionRepository reactionRepository, IMapper mapper, IFakeLogger logger, LinkGenerator linkGenerator, 
                                  IHttpContextAccessor contextAccessor, IPostService postService, IReactionTypeRepository reactionTypeRepository)
        {
            _reactionRepository = reactionRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _postService = postService;
            _reactionTypeRepository = reactionTypeRepository;
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
                if (reaction == null || reaction.Count == 0)
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
        /// <param name="userId">User's Id</param>
        /// <returns> Reaction(s) with postId</returns>
        /// <remarks>        
        /// Example of a request to get reactions(s) by postId
        /// GET 'https://localhost:44389/api/reactions/postId/8CCB1467-9F38-4164-88DA-15882FE82E58' \
        ///     --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJLZXkiOiJTZWNyZXRLZXlEdXNhbktyc3RpYzEyMyIsInJvbGUiOiJVc2VyIn0.4cCC6M5FbRuEDgB09F_9T-3To760pEx6ZXKEqrKsKxg' \
        /// Successful request: --param 'userId = 59ed7d80-39c9-42b8-a822-70ddd295914a'
        /// Bad request:        --param 'userId = f2f88bcd-d0a2-4fe7-a23f-df97a59731cd'
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
        public ActionResult<List<Reaction>> GetReactionByPostId(Guid postId, [FromHeader] Guid userId)
        {
            try
            {
                var post = _postService.GetPostById<PostDto>(HttpMethod.Get, postId, Request.Headers["Authorization"]).Result;
                if (post == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, String.Format("Post with ID {0} does not exist!", postId));
                }
                var sellerId = post.AccountId;

                if (_reactionRepository.CheckDidIBlockSeller(userId, sellerId))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "You can't see reactions for this post because you blocked seller or you are blocked!");
                }

                var reaction = _reactionRepository.GetReactionByPostId(postId, userId);
                if (reaction == null || reaction.Count == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "There are no reactions for this post yet");
                }
                return Ok(reaction);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        /// <summary>
        /// Creates reaction 
        /// </summary>
        /// <param name="reactionDto">Model of reaction to create</param>
        /// <param name="userId">Id of the user who sends the request</param>
        /// <remarks>
        /// POST 'https://localhost:44389/api/reactions/' \
        /// Example of a request to create reaction \
        ///  --header 'Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJLZXkiOiJTZWNyZXRLZXlEdXNhbktyc3RpYzEyMyIsInJvbGUiOiJVc2VyIn0.4cCC6M5FbRuEDgB09F_9T-3To760pEx6ZXKEqrKsKxg' \
        ///  Successful request: 
        ///  --param 'userId = 59ed7d80-39c9-42b8-a822-70ddd295914a'
        /// {
        /// "postId": "23d2cce9-86d7-4bff-887e-f7712b16766d",
        /// "reactionTypeId": 2,
        /// }
        ///  Bad request if previous request has already been executed because you can react to the same post only once: 
        ///  --param 'userId = 59ed7d80-39c9-42b8-a822-70ddd295914a'
        /// {
        /// "postId": "23d2cce9-86d7-4bff-887e-f7712b16766d",
        /// "reactionTypeId": 3,
        /// }
        ///  Bad request because user with ID: '59ed7d80-39c9-42b8-a822-70ddd295914a' doesn't follow the seller of this post and can't react:
        ///  --param 'userId = 59ed7d80-39c9-42b8-a822-70ddd295914a'
        /// {
        /// "postId": "5284A73F-1F9E-4799-A793-5A4FE4A1DF56",
        /// "reactionTypeId": 3,
        /// }
        ///  Bad request because reaction type with ID: '20' doesn't exist :
        ///  --param 'userId = 59ed7d80-39c9-42b8-a822-70ddd295914a'
        /// {
        /// "postId": "23d2cce9-86d7-4bff-887e-f7712b16766d",
        /// "reactionTypeId": 20,
        /// }
        ///  Bad request because post with ID: '23d2cce9-86d7-4bff-887e-f7712b16766c' doesn't exist :
        ///  --param 'userId = 59ed7d80-39c9-42b8-a822-70ddd295914a'
        /// {
        /// "postId": "23d2cce9-86d7-4bff-887e-f7712b16766c",
        /// "reactionTypeId": 2,
        /// }
        /// </remarks>
        /// <response code="201">Returns the created reaction</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="409">Conflict - Foreign key constraint violation</response>
        /// <response code="500">There was an error on the server</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public ActionResult<Reaction> CreateReaction([FromBody] ReactionCreationDto reactionDto, [FromHeader] Guid userId)
        {
            try
            {
                Reaction reaction = _mapper.Map<Reaction>(reactionDto);

                ReactionType reactionType = _reactionTypeRepository.GetReactionTypeById(reaction.ReactionTypeId);
                if (reactionType == null)
                {
                    throw new ForeignKeyConstraintException("Foreign key constraint violated. Reaction type with that ID doesn't exist!");
                }

                var post = _postService.GetPostById<PostDto>(HttpMethod.Get, reaction.PostId, Request.Headers["Authorization"]).Result;
                if (post == null)
                {
                    throw new ForeignKeyConstraintException("Foreign key constraint violated. Post with that ID doesn't exist!");
                }
                var sellerId = post.AccountId;

                if (!_reactionRepository.CheckDoIFollowSeller(userId, sellerId))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, String.Format("You are not following user with ID {0} so you can not react to his posts.", sellerId));
                }

                if (_reactionRepository.CheckDidIAlreadyReact(userId, reaction.PostId) != null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "You have already reacted to this post.");
                }

                reaction.AccountId = userId;

                _reactionRepository.CreateReaction(reaction);
                _reactionRepository.SaveChanges();

                _logger.Log(LogLevel.Information, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully created new reaction with ID {0} in database", reaction.ReactionId), null);

                string location = _linkGenerator.GetPathByAction("GetReactionById", "Reaction", new { reactionId = reaction.ReactionId });
                return Created(location, reaction);
            }
            catch (Exception ex)
            {
                if (ex.GetType().IsAssignableFrom(typeof(ForeignKeyConstraintException)))
                {
                    return StatusCode(StatusCodes.Status409Conflict, ex.Message);
                }
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while creating reaction, message: {0}", ex.Message), null);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates reaction
        /// </summary>
        /// <param name="reactionUpdateDto">Model of reaction for udpdate</param>
        /// <param name="reactionId">Reaction id</param>
        /// <param name="userId">Id of the user who sends the request</param>
        /// <returns>Confirmation of update</returns>
        /// <remarks>
        /// PUT 'https://localhost:44389/api/reactions/3B6D0A06-E64B-4F42-8689-FC10E8E6EDF7' \
        ///  Successful request: 
        ///  --param 'userId = 42B70088-9DBD-4B19-8FC7-16414E94A8A6'
        /// {
        /// "reactionTypeId": 2,
        /// }
        /// Bad request because user with id: '59ED7D80-39C9-42B8-A822-70DDD295914A' didn't create this reaction:
        ///  --param 'userId = 59ED7D80-39C9-42B8-A822-70DDD295914A'
        /// {
        /// "reactionTypeId": 2,
        /// }
        /// </remarks>
        /// <response code="200">Returns updated reaction</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="403">Forbiden request - user with this userId doesn't have permission to update reaction</response>
        /// <response code="404">Post history with postHistoryId is not found</response>
        /// <response code="409">Conflict - Foreign key constraint violation</response>
        /// <response code="500">Error on the server while updating</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{reactionId}")]
        public ActionResult<Reaction> UpdateReaction([FromBody] ReactionUpdateDto reactionUpdateDto, Guid reactionId, [FromHeader] Guid userId)
        {
            try
            {
                var oldReaction = _reactionRepository.GetReactionById(reactionId);
                if (oldReaction == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, String.Format("There is no reaction with ID {0}", reactionId));
                }
                Reaction newReaction = _mapper.Map<Reaction>(reactionUpdateDto);
                newReaction.PostId = oldReaction.PostId;

                if (oldReaction.AccountId != userId)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, String.Format("User whit ID {0} doesn't have permission to update this reaction because he didn't create it", userId));
                }

                ReactionType reactionType = _reactionTypeRepository.GetReactionTypeById(newReaction.ReactionTypeId);
                if (reactionType == null)
                {
                    throw new ForeignKeyConstraintException("Foreign key constraint violated. Reaction type with that ID doesn't exist!");
                }

                var post = _postService.GetPostById<PostDto>(HttpMethod.Get, oldReaction.PostId, Request.Headers["Authorization"]).Result;
                if (post == null)
                {
                    throw new ForeignKeyConstraintException("Foreign key constraint violated. Post with that ID doesn't exist!");
                }

                _reactionRepository.UpdateReaction(oldReaction, newReaction);
                _reactionRepository.SaveChanges();
                _logger.Log(LogLevel.Information, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully updated reaction with ID {0}", reactionId), null);

                return Ok(oldReaction);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while updating reaction with ID {0}, message: {1}", reactionId, ex.Message), null);
                if (ex.GetType().IsAssignableFrom(typeof(ForeignKeyConstraintException)))
                {
                    return StatusCode(StatusCodes.Status409Conflict, ex.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Deleting reaction with reactionId
        /// </summary>
        /// <param name="reactionId">Reaction's Id</param>
        /// <param name="userId">ID of the user who sends the request</param>
        /// <returns>Status 204 - NoContent</returns>
        /// <remarks>        
        /// Example of a request to delete reaction
        /// DELETE 'https://localhost:44389/api/reactions/19e0acbf-5707-49ee-8cb6-134c00b7c10b' \
        /// Successful request:
        /// --param userId = 'f2f88bcd-d0a2-4fe7-a23f-df97a59731cd'
        /// Bad request because user with id: '42b70088-9dbd-4b19-8fc7-16414e94a8a6' didn't create this reaction:
        /// --param userId = '42b70088-9dbd-4b19-8fc7-16414e94a8a6'
        /// </remarks>
        /// <response code="204">Reaction successfully deleted</response>
        /// <response code="401" >Unauthorized user</response>
        /// <response code="403">Forbiden request - user with this userId doesn't have permission to delete reaction</response>
        /// <response code="404">Reaction with this reactionId is not found</response>
        /// <response code="500">There was an error on the server</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{reactionId}")]
        public IActionResult DeleteReaction(Guid reactionId, [FromHeader] Guid userId)
        {
            try
            {
                var reaction = _reactionRepository.GetReactionById(reactionId);

                if (reaction.AccountId != userId)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, String.Format("User whit ID {0} doesn't have permission to delete this reaction because he didn't create it", userId));
                }

                if (reaction == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, String.Format("There is no reaction with ID {0}", reactionId));
                }

                _reactionRepository.DeleteReaction(reactionId);
                _reactionRepository.SaveChanges();
                _logger.Log(LogLevel.Information, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully deleted reaction with ID {0} from database", reactionId), null);

                return NoContent();
            }

            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while deleting reaction with ID {0}, message: {1}", reactionId, ex.Message), null);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting reaction!");
            }
        }
    }
}
