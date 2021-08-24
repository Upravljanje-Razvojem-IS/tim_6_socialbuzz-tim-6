using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using UserService.DTOs.CoorporateDtos;
using UserService.Interfaces;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/coorporate")]
    public class CoorporateController : ControllerBase
    {
        private readonly ICoorporateRepository _repository;

        public CoorporateController(ICoorporateRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all coorporate accounts
        /// </summary>
        /// <returns>list of coorporate accounts</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpGet]
        public ActionResult GetAll()
        {
            var entities = _repository.Get();

            if (entities.Count == 0)
                return NoContent();

            return Ok(entities);
        }

        /// <summary>
        /// Get coorporate account by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>coorporate account</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult GetById(Guid id)
        {
            var entity = _repository.Get(id);

            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        /// <summary>
        /// Create coorporate account
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>new account</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public ActionResult PostCoorporate([FromBody] CoorporatePostDto dto)
        {
            var entity = _repository.Create(dto);

            return Ok(entity);
        }

        /// <summary>
        /// Update coorporate account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns>updated acc</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult PutCoorporate(Guid id, CoorporatePutDto dto)
        {
            var entity = _repository.Update(id, dto);

            return Ok(entity);
        }

        /// <summary>
        /// Delete coorporate account by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpDelete]
        public ActionResult DeleteCoorporate(Guid id)
        {
            _repository.Delete(id);

            return NoContent();
        }
    }
}
