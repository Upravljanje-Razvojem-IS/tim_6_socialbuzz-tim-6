using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using UserService.DTOs.PersonalDtos;
using UserService.Interfaces;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/personal")]
    public class PersonalController : ControllerBase
    {
        private readonly IPersonalRepository _repository;

        public PersonalController(IPersonalRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all perosnal accounts
        /// </summary>
        /// <returns>list of personal accounts</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
        /// Get personal account by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>perosnal account</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// Create perosnal account
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>new account</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public ActionResult PostPersonal([FromBody] PersonalPostDto dto)
        {
            var entity = _repository.Create(dto);

            return Ok(entity);
        }

        /// <summary>
        /// Update personal account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns>updated acc</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult PutPersonal(Guid id, PersonalPutDto dto)
        {
            var entity = _repository.Update(id, dto);

            return Ok(entity);
        }

        /// <summary>
        /// Delete personal account by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        [HttpDelete]
        public ActionResult DeletePersonal(Guid id)
        {
            _repository.Delete(id);

            return NoContent();
        }
    }
}
