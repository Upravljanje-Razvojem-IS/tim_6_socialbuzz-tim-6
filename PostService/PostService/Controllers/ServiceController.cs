using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostService.Data.Post;
using PostService.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Controllers
{
    /// <summary>
    /// Service controller with endpoints for CRUD operations
    /// </summary>
    [ApiController]
    [Route("api/services")]
    [Produces("application/json", "application/xml")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        /// <summary>
        /// Returns implemented options for API   
        /// </summary>
        /// <returns>Header key 'Allow' with allowed requests</returns>
        /// <remarks>
        /// Example of successful request \
        /// OPTIONS 'https://localhost:44377/api/services' \
        /// </remarks>
        /// <response code="200">Return header key 'Allow' with allowed requests</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpOptions]
        public IActionResult GetPostOpstions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }

        /// <summary>
        /// Returns list of all services
        /// </summary>
        /// <param name="serviceName">Name of the service</param>
        /// <returns>List of products</returns>
        /// <response code="200">Returns list of services</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">No services found</response>
        /// <response code="500">There was an error on the server</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Service>> GetServices(string serviceName)
        {
            try
            {
                var services = _serviceRepository.GetServices(serviceName);
                if (services == null || services.Count == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "No services found");
                }
                return Ok(services);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Returns service with serviceId
        /// </summary>
        /// <param name="serviceId">Service's Id</param>
        /// <returns>Service with serviceId</returns>
        ///<response code="200">Returns the service</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">Service with this serviceId is not found</response>
        /// <response code="500">There was an error on the server</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{serviceId}")]
        public ActionResult<Service> GetServiceById(Guid serviceId)
        {
            try
            {
                var service = _serviceRepository.GetServiceById(serviceId);
                if (service == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Service with this id is not found");
                }
                return Ok(service);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }
    }
}
