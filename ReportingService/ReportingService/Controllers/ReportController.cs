using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportingService.DTOs;
using ReportingService.Interfaces;
using System;

namespace ReportingService.Controllers
{
    [ApiController]
    [Route("api/report")]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _repository;

        public ReportController(IReportRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all reports
        /// </summary>
        /// <returns>List of reports</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public ActionResult GetAllReports()
        {
            var entities = _repository.GetAll();

            if (entities.Count == 0)
                return NoContent();

            return Ok(entities);
        }

        /// <summary>
        /// Report by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Report</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public ActionResult GetById(Guid id)
        {
            var entity = _repository.GetById(id);

            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        /// <summary>
        /// Post new Report
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>Report confirmation</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public ActionResult PostReport([FromBody] ReportCreate dto)
        {
            var entity = _repository.PostNew(dto);

            return Ok(entity);
        }

        /// <summary>
        /// Update report
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns>report confirmation</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}")]
        public ActionResult PutReport(Guid id, ReportCreate dto)
        {
            var entity = _repository.Update(id, dto);

            return Ok(entity);
        }

        /// <summary>
        /// Delete report by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete]
        public ActionResult DeleteReport(Guid id)
        {
            _repository.Delete(id);

            return NoContent();
        }
    }
}
