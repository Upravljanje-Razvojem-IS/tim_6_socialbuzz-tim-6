using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using ReactionService.Data.ReactionTypes;
using ReactionService.Entities;
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

        public ReactionTypeController(IReactionTypeRepository reactionTypeRepository, IMapper mapper, IFakeLogger logger, IHttpContextAccessor contextAccessor, LinkGenerator linkGenerator)
        {
            _reactionTypeRepository = reactionTypeRepository;
            _mapper = mapper;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _linkGenerator = linkGenerator;
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
        /// <response code="500">There was an error on the server</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
    }
}
