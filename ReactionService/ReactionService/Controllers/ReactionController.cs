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
                    return StatusCode(StatusCodes.Status400BadRequest, "Post with that ID does not exist!");
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
    }
}
