using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using ReactionService.Data.Reactions;
using ReactionService.Data.ReactionTypes;
using ReactionService.Entities;
using ReactionService.Exceptions;
using ReactionService.Logger;
using ReactionService.Models.DTOs.ReactionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Controllers
{
    /// <summary>
    /// ReactionType controller with endpoints for fetching different types of reactions
    /// </summary>
    [ApiController]
    [Route("api/reactionTypes")]
    [Produces("application/json", "application/xml")]
    public class ReactionTypeController : ControllerBase
    {
        private readonly IReactionTypeRepository _reactionTypeRepository;
        private readonly IMapper _mapper;
        private readonly IFakeLogger _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly LinkGenerator _linkGenerator;
        private readonly IReactionRepository _reactionRepository;

        public ReactionTypeController(IReactionTypeRepository reactionTypeRepository, IMapper mapper, IFakeLogger logger,
                                      IReactionRepository reactionRepository, IHttpContextAccessor contextAccessor, LinkGenerator linkGenerator)
        {
            _reactionTypeRepository = reactionTypeRepository;
            _mapper = mapper;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _linkGenerator = linkGenerator;
            _reactionRepository = reactionRepository;
        }

        /// <summary>
        /// Returns implemented options for API   
        /// </summary>
        /// <returns>Header key 'Allow' with allowed requests</returns>
        /// <remarks>
        /// Example of successful request \
        /// OPTIONS 'https://localhost:44389/api/reactionTypes' \
        /// </remarks>
        /// <response code="200">Return header key 'Allow' with allowed requests</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpOptions]
        public IActionResult GetReactionTypesOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }

        /// <summary>
        /// Returns list of all reaction types
        /// </summary>
        /// <returns>List of reaction types</returns>
        /// <remarks>
        /// GET 'https://localhost:44389/api/reactionTypes' \
        ///     Example of successful request \
        ///         --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJLZXkiOiJTZWNyZXRLZXlEdXNhbktyc3RpYzEyMyIsInJvbGUiOiJVc2VyIn0.4cCC6M5FbRuEDgB09F_9T-3To760pEx6ZXKEqrKsKxg' \
        /// </remarks>
        /// <response code="200">Returns list of reaction types</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">No reaction types found</response>
        /// <response code="500">There was an error on the server</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin, User")]
        public ActionResult<List<ReactionType>> GetReactionTypes()
        {
            try
            {
                var reactionTypes = _reactionTypeRepository.GetReactionTypes();
                if (reactionTypes == null || reactionTypes.Count == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "No reaction types found");
                }
                return Ok(reactionTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Returns reaction type with reactionTypeId
        /// </summary>
        /// <param name="reactionTypeId">Reaction type's Id</param>
        /// <returns>Reaction type with reactionTypeId</returns>
        /// <remarks>
        /// GET 'https://localhost:44389/api/reactionTypes/2' \
        ///     Example of successful request \
        ///         --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJLZXkiOiJTZWNyZXRLZXlEdXNhbktyc3RpYzEyMyIsInJvbGUiOiJVc2VyIn0.4cCC6M5FbRuEDgB09F_9T-3To760pEx6ZXKEqrKsKxg' \
        /// </remarks>
        ///<response code="200">Returns the reaction type</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">Reaction type with this reactionTypeId is not found</response>
        /// <response code="500">There was an error on the server</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{reactionTypeId}")]
        [Authorize(Roles = "Admin, User")]
        public ActionResult<ReactionType> GetReactionTypeById(int reactionTypeId)
        {
            try
            {
                var reactionType = _reactionTypeRepository.GetReactionTypeById(reactionTypeId);
                if (reactionType == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Reaction type with this id is not found");
                }
                return Ok(reactionType);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        /// <summary>
        /// Creates reaction type
        /// </summary>
        /// <param name="reactionTypeDto">Model of reactionType to create</param>
        /// <remarks>
        /// POST 'https://localhost:44389/api/reactionTypes/' \
        /// Example of a request to create reactionType \
        ///  --header 'Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJLZXkiOiJTZWNyZXRLZXlEdXNhbktyc3RpYzEyMyIsInJvbGUiOiJVc2VyIn0.4cCC6M5FbRuEDgB09F_9T-3To760pEx6ZXKEqrKsKxg' \
        /// {
        /// "typeName": "NewType"
        /// }
        /// </remarks>
        /// <response code="201">Returns the created reactionType</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="403">Forbiden request - user with this userId doesn't have permission to create reaction type</response>
        /// <response code="500">There was an error on the server</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ReactionTypeConfirmationDto> CreateReactionType([FromBody] ReactionTypeCreationDto reactionTypeDto)
        {
            try
            {
                ReactionType reactionType = _mapper.Map<ReactionType>(reactionTypeDto);
                _reactionTypeRepository.CreateReactionType(reactionType);
                _reactionTypeRepository.SaveChanges();

                _logger.Log(LogLevel.Information, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully created new reaction type with ID {0} in database", reactionType.ReactionTypeId), null);
                
                string location = _linkGenerator.GetPathByAction("GetReactionTypeById", "ReactionType", new { reactionTypeId = reactionType.ReactionTypeId });
                return Created(location, _mapper.Map<ReactionTypeConfirmationDto>(reactionType));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while creating reaction type, message: {0}", ex.Message), null);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates reaction type
        /// </summary>
        /// <param name="reactionTypeUpdateDto">Model of reaction type for udpdate</param>
        /// <param name="reactionTypeId">Reaction type id</param>
        /// <returns>Confirmation of update</returns>
        /// <remarks>
        /// POST 'https://localhost:44389/api/reactionTypes/' \
        /// Example of a request to update product \
        ///  --header 'Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJLZXkiOiJTZWNyZXRLZXlEdXNhbktyc3RpYzEyMyIsInJvbGUiOiJVc2VyIn0.4cCC6M5FbRuEDgB09F_9T-3To760pEx6ZXKEqrKsKxg' \
        ///  --param  'reactionTypeId = example of Guid'
        /// {
        /// "typeName": "UpdatedType"
        /// }
        /// </remarks>
        /// <response code="200">Returns updated reaction type</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="403">Forbiden request - user with this userId doesn't have permission to update reaction type</response>
        /// <response code="404">Reaction type with reactionTypeId is not found</response>
        /// <response code="409">Conflict - Foreign key constraint violation</response>
        /// <response code="500">Error on the server while updating</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{reactionTypeId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ReactionType> UpdateReactionType([FromBody] ReactionTypeUpdateDto reactionTypeUpdateDto, int reactionTypeId)
        {
            try
            {
                var oldReactionType = _reactionTypeRepository.GetReactionTypeById(reactionTypeId);
                if (oldReactionType == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, String.Format("There is no reactionType with ID {0}", reactionTypeId));
                }
                ReactionType newReactionType = _mapper.Map<ReactionType>(reactionTypeUpdateDto);
                _reactionTypeRepository.UpdateReactionType(oldReactionType, newReactionType);
                _reactionTypeRepository.SaveChanges();

                _logger.Log(LogLevel.Information, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully updated reaction type with ID {0}", reactionTypeId), null);

                return Ok(oldReactionType);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while updating reaction type with ID {0}, message: {1}", reactionTypeId, ex.Message), null);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Deleting reaction type with reactionTypeId
        /// </summary>
        /// <param name="reactionTypeId">Reaction type's Id</param>
        /// <returns>Status 204 - NoContent</returns>
        /// <remarks>        
        /// Example of a request to delete reaction type
        /// DELETE 'https://localhost:44389/api/reactionTypes' \
        ///   --param  'reactionTypeId = 4'
        ///   --param  userId 'userId = 42b70088-9dbd-4b19-8fc7-16414e94a8a6'
        ///            wrong userId 'userId = 59ed7d80-39c9-42b8-a822-70ddd295914a'
        /// </remarks>
        /// <response code="204">Reaction type successfully deleted</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="403">Forbiden request - user with this userId doesn't have permission to delete reaction type</response>
        /// <response code="404">Reaction type with this reactionTypeId is not found</response>
        /// <response code="409">Reaction type reference in another table</response>
        /// <response code="500">There was an error on the server</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{reactionTypeId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteReactionType(int reactionTypeId)
        {
            try
            {
                var reactionType = _reactionTypeRepository.GetReactionTypeById(reactionTypeId);

                if (reactionType == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, String.Format("There is no reactionType with ID {0}!", reactionTypeId));
                }

                List<Reaction> reactions = _reactionRepository.GetReactionByReactionTypeId(reactionTypeId);
                if (reactions.Count > 0)
                {
                    throw new ReferentialConstraintException("Value of reaction type is referenced in another table");
                }
                _reactionTypeRepository.DeleteReactionType(reactionTypeId);
                _reactionTypeRepository.SaveChanges();

                return NoContent();
            }

            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while deleting reaction type with ID {0}, message: {1}", reactionTypeId, ex.Message), null);
                if (ex.GetBaseException().GetType() == typeof(ReferentialConstraintException))
                {
                    return StatusCode(StatusCodes.Status409Conflict, ex.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting reaction type!");
            }
        }
    }
}
