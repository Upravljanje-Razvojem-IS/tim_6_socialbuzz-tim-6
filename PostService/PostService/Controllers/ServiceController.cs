using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using PostService.Data.Post;
using PostService.Data.PostHistories;
using PostService.Entities;
using PostService.Entities.Posts;
using PostService.Exceptions;
using PostService.Logger;
using PostService.Models.DTOs.Post;
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
    [Authorize]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        private readonly IFakeLogger _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPostHistoryRepository _postHistoryRepository;

        public ServiceController(IServiceRepository serviceRepository, IMapper mapper, IFakeLogger logger,
                                    LinkGenerator linkGenerator, IHttpContextAccessor contextAccessor, IPostHistoryRepository postHistoryRepository)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _postHistoryRepository = postHistoryRepository;
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
        [AllowAnonymous]
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
        [AllowAnonymous]
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

        /// <summary>
        /// Creates service
        /// </summary>
        /// <param name="serviceDto">Model of service to create</param>
        /// <remarks>
        /// POST 'https://localhost:44377/api/services/' \
        /// Example of a request to create service \
        ///  --header 'Authorization: TODO - dodati jwt' \
        /// {
        /// "postName": "Sportska masaža",
        /// "postImage": "https://www.msccollege.edu/wp-content/uploads/2020/06/sports-massage-therapy-1024x683.jpg",
        /// "description": "Zakaži svoj termin i spremno dočekaj sledeći trening",
        /// "price": 3000,
        /// "currency": "Rsd",
        /// "category": "Lepota i zdravlje",
        /// "accountId": "59ed7d80-39c9-42b8-a822-70ddd295914a"
        /// }
        /// </remarks>
        /// <response code="201">Returns the created service</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="500">There was an error on the server</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public ActionResult<ServiceConfirmationDto> CreateService([FromBody] ServiceCreationDto serviceDto)
        {
            try
            {
                Service service = _mapper.Map<Service>(serviceDto);
                service.PublicationDate = DateTime.Now;
                _serviceRepository.CreateService(service);
                _serviceRepository.SaveChanges();
                _logger.Log(LogLevel.Information, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully created new service with ID {0} in database", service.PostId), null);
                string location = _linkGenerator.GetPathByAction("GetServiceById", "Service", new { serviceId = service.PostId });
                return Created(location, _mapper.Map<ServiceConfirmationDto>(service));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while creating service, message: {0}", ex.Message), null);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates service
        /// </summary>
        /// <param name="serviceUpdateDto">Model of service for udpdate</param>
        /// <param name="serviceId">Service id</param>
        /// <param name="userId">ID of the user who sends the request/param>
        /// <returns>Confirmation of update</returns>
        /// <remarks>
        /// POST 'https://localhost:44377/api/services/' \
        /// Example of a request to update service \
        ///  --header 'Authorization: TODO - dodati jwt' \
        ///  --param  'serviceId = example of Guid'
        ///  --param  correct userId 'userId = 59ed7d80-39c9-42b8-a822-70ddd295914a'
        ///           wrong userId 'userId = 42b70088-9dbd-4b19-8fc7-16414e94a8a6'
        /// {
        /// "postName": "Masaža za sportiste",
        /// "postImage": "https://clubfiftyone.co.uk/wp-content/uploads/2019/09/Sports-therapy.jpg",
        /// "description": "Zakaži svoj termin za sportsku masažu i spremno dočekaj sledeći trening",
        /// "price": 2800,
        /// "currency": "Rsd",
        /// "category": "Lepota i zdravlje",
        /// "accountId": "59ed7d80-39c9-42b8-a822-70ddd295914a"
        /// }
        /// </remarks>
        /// <response code="200">Returns updated service</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="403">Forbiden request - user with this userId doesn't have permission to update service</response>
        /// <response code="404">Service with serviceId is not found</response>
        /// <response code="409">Conflict - Foreign key constraint violation</response>
        /// <response code="500">Error on the server while updating</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{serviceId}")]
        public ActionResult<Service> UpdateService([FromBody] ServiceUpdateDto serviceUpdateDto, Guid serviceId, [FromHeader] Guid userId)
        {
            try
            {
                if (serviceUpdateDto.AccountId != userId)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, String.Format("User whit ID {0} doesn't have permission to update this service because he isn't service owner", userId));
                }

                var oldService = _serviceRepository.GetServiceById(serviceId);
                if (oldService == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, String.Format("There is no service with ID {0}", serviceId));
                }
                Service newService = _mapper.Map<Service>(serviceUpdateDto);

                _serviceRepository.UpdateService(oldService, newService);
                _serviceRepository.SaveChanges();
                _logger.Log(LogLevel.Information, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully updated service with ID {0}", serviceId), null);

                return Ok(oldService);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while updating service with ID {0}, message: {1}", serviceId, ex.Message), null);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Deleting service with serviceId
        /// </summary>
        /// <param name="serviceId">Service's Id</param>
        /// <param name="userId">ID of the user who sends the request/param>
        /// <returns>Status 204 - NoContent</returns>
        /// <remarks>        
        /// Example of a request to delete service
        /// DELETE 'https://localhost:44377/api/services' \
        ///   --param  'serviceId = 4767dabb-05ac-4db6-d37a-08d94f34a631'
        ///   --param  userId 'userId = 59ed7d80-39c9-42b8-a822-70ddd295914a'
        ///            wrong userId 'userId = 42b70088-9dbd-4b19-8fc7-16414e94a8a6'
        /// </remarks>
        /// <response code="204">Service successfully deleted</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="403">Forbiden request - user with this userId doesn't have permission to delete service</response>
        /// <response code="404">Service with this serviceId is not found</response>
        /// <response code="409">Service reference in another table</response>
        /// <response code="500">There was an error on the server</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{serviceId}")]
        public IActionResult DeleteService(Guid serviceId, [FromHeader] Guid userId)
        {
            try
            {
                var service = _serviceRepository.GetServiceById(serviceId);

                if (service.AccountId != userId)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, String.Format("User whit ID {0} doesn't have permission to delete this service because he isn't service owner", userId));
                }

                if (service == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, String.Format("There is no service with ID {0}!", serviceId));
                }

                List<PostHistory> postHistories = _postHistoryRepository.GetPostHistoryByPostId(serviceId);
                if (postHistories.Count > 0)
                {
                    throw new ReferentialConstraintException("Value of service is referenced in another table");
                }
                _serviceRepository.DeleteService(serviceId);
                _serviceRepository.SaveChanges();

                return NoContent();
            }

            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while deleting service with ID {0}, message: {1}", serviceId, ex.Message), null);
                if (ex.GetBaseException().GetType() == typeof(ReferentialConstraintException))
                {
                    return StatusCode(StatusCodes.Status409Conflict, ex.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting service!");
            }
        }
    }
}
