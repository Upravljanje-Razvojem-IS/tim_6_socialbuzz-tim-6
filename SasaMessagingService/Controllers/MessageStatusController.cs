using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SasaMessagingService.DTOs.MessageStatusDtos;
using SasaMessagingService.Interfaces;
using System;

namespace SasaMessagingService.Controllers
{
    [ApiController]
    [Route("api/message-status")]
    [Authorize]
    public class MessageStatusController : ControllerBase
    {
        private readonly IMessageStatusRepository _repository;

        public MessageStatusController(IMessageStatusRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all statuses
        /// </summary>
        /// <returns>List of message statuses</returns>
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
        /// Get status by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Message Status</returns>
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
        /// Create new message status
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>new status</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public ActionResult PostMessageStatus([FromBody] MessageStatusPostDto dto)
        {
            var entity = _repository.Create(dto);

            return Ok(entity);
        }

        /// <summary>
        /// Update status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns>updated status</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}")]
        public ActionResult PutMessageStatus(Guid id, MessageStatusPutDto dto)
        {
            var entity = _repository.Update(id, dto);

            return Ok(entity);
        }

        /// <summary>
        /// Delete status 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete]
        public ActionResult DeleteMessageStatus(Guid id)
        {
            _repository.Delete(id);

            return NoContent();
        }
    }
}
