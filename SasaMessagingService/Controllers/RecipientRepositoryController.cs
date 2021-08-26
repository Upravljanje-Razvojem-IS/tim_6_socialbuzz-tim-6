using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SasaMessagingService.DTOs.RecipientDtos;
using SasaMessagingService.Interfaces;
using System;

namespace SasaMessagingService.Controllers
{
    [ApiController]
    [Route("api/recipient")]
    [Authorize]
    public class RecipientController : ControllerBase
    {
        private readonly IRecipientRepository _repository;

        public RecipientController(IRecipientRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all recipients
        /// </summary>
        /// <returns>List of all</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public ActionResult GetAll()
        {
            var entities = _repository.Get();

            if (entities.Count == 0)
                return NoContent();

            return Ok(entities);
        }

        /// <summary>
        /// Get recipient by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Single recipient</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public ActionResult GetById(Guid id)
        {
            var entity = _repository.Get(id);

            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        /// <summary>
        /// Create new Recipient
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>New recipient</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public ActionResult PostRecipiet([FromBody] RecipientPostDto dto)
        {
            var entity = _repository.Create(dto);

            return Ok(entity);
        }

        /// <summary>
        /// Update recipient
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns>Updated</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}")]
        public ActionResult PutRecipiet(Guid id, RecipientPutDto dto)
        {
            var entity = _repository.Update(id, dto);

            return Ok(entity);
        }

        /// <summary>
        /// Delete recipient by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete]
        public ActionResult DeleteRecipiet(Guid id)
        {
            _repository.Delete(id);

            return NoContent();
        }
    }
}
