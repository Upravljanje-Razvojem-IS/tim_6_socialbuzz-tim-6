using EvaluationsService.Data.PostMock;
using EvaluationsService.Model.Enteties;
using LoggingClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvaluationsService.Auth;
using EvaluationsService.Data;
using EvaluationsService.Data.Mocks.AccountMock;
using EvaluationsService.Model.ValueObjects;
using EvaluationsService.Model;
using Microsoft.Extensions.Logging;

namespace EvaluationsService.Controllers
{
    [ApiController]
    [Route("api/evaluations")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class EvaluationsController : ControllerBase
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IEvaluationsRepository evaluationRepository;
        private readonly IAccountMockRepository accountRepository;
        private readonly IPostMockRepository postRepository;
        private readonly IConfiguration configuration;
        private readonly Logger logger;

        public EvaluationsController(IHttpContextAccessor contextAccessor, IPostMockRepository postRepository, IAccountMockRepository accountRepository, IEvaluationsRepository evaluationRepository, Logger logger, IConfiguration configuration)
        {
            this.contextAccessor = contextAccessor;
            this.evaluationRepository = evaluationRepository;
            this.postRepository = postRepository;
            this.accountRepository = accountRepository;
            this.configuration = configuration;
            this.logger = logger;
        }


        private bool Authorize(string key)
        {
            if (key == null)
            {
                return false;
            }

            if (!key.StartsWith("Bearer"))
            {
                return false;
            }

            var keyOnly = key.Substring(key.IndexOf("Bearer") + 7);
            var username = configuration.GetValue<string>("Authorization:Username");
            var password = configuration.GetValue<string>("Authorization:Password");
            var base64EncodedBytes = System.Convert.FromBase64String(keyOnly);
            var user = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

            if ((username + ":" + password) != user)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Return implemented options for API   
        /// </summary>
        /// <returns>Header key 'Allow' with allowed requests</returns>
        /// <remarks>
        /// Example of successful request \
        /// OPTIONS 'https://localhost:32647/api/evaluations' \
        /// </remarks>
        /// <response code="200">Return header key 'Allow' with allowed requests</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpOptions]
        public IActionResult GetCommentsOpstions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }

        /// <summary>
        /// Return all evaluations    
        /// </summary>
        /// <param name="key">Authorization header</param>
        /// <returns>List of all evaluations</returns>
        /// <remarks>
        /// Example of successful request \
        /// GET 'https://localhost:32647/api/evaluations' \
        ///     --header 'Authorization: Bearer YWRtaW46c3VwZXJBZG1pbjEyMw=='
        /// </remarks>
        /// <response code="200">Return list of evaluations</response>
        /// <response code="404">There is no evaluations</response>
        /// <response code="401">Authorization error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public ActionResult<List<Evaluation>> GetAllEvaluations([FromHeader(Name = "Authorization")] string key)
        {
            if (!Authorization.Authorize(key,configuration))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = ""
                });
            }
            var evaluations = evaluationRepository.GetAllEvaluations();
            if (evaluations == null || evaluations.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, "There is no evaluations.");
            }

            return StatusCode(StatusCodes.Status200OK, new { status = "OK", content = evaluations });
        }

        /// <summary>
        /// Return evaluation for provided evaluation ID    
        /// </summary>
        /// <returns>Evaluation with provided ID</returns>
        /// <remarks>
        /// Example of successful request \
        /// GET 'https://localhost:32647/api/evaluations/da0fdb58-6b1f-43e1-97d5-55d1089f5142' \
        ///     --header 'Authorization: Bearer YWRtaW46c3VwZXJBZG1pbjEyMw=='
        /// </remarks>
        /// <response code="200">Returns evaluation for provided ID</response>
        /// <response code="400">There is no evaluation with provided ID</response>
        /// <response code="401">Authorization error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{evaluationID}")]
        public ActionResult<Evaluation> GetEvaluationByID([FromHeader(Name = "Authorization")] string key, [FromRoute] Guid evaluationID)
        {
            if (!Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = ""
                });
            }

            var evaluation = evaluationRepository.GetEvaluationByID(evaluationID);

            if (evaluation == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { status = "There is no evaluation with provided ID", content = "" });
            }

            return StatusCode(StatusCodes.Status200OK, new { status = "OK", content = evaluation });
        }

        /// <summary>
        /// Return evaluations from the specified post 
        /// </summary>
        /// <returns>List of evaluations for the specified post</returns>
        /// <remarks>
        /// GET 'https://localhost:32647/api/evaluations/byPostID' \
        ///     Example of successful request \
        ///         --header 'Authorization: Bearer YWRtaW46c3VwZXJBZG1pbjEyMw==' \
        ///         --param  'postID = 1' \
        ///     Example of a failed request \
        ///         --header 'Authorization: Bearer YWRtaW46c3VwZXJBZG1pbjEyMw==' \
        ///         --param  'postID = 3' \
        /// </remarks>
        /// <param name="postID">Post ID</param>
        /// <param name="key">Authorization header</param>
        /// <response code="200">Return list of evaluations for given post</response>
        /// <response code="400">Post with provided ID does not exist</response>
        /// <response code="401">Authorization error</response>
        /// <response code="404">There is no evaluations with provided post ID</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("byPostID")]
        public ActionResult<List<Evaluation>> GetEvaluationsByPostID([FromHeader(Name = "Authorization")] string key, [FromQuery] int postID)
        {
            if (!Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = ""
                });
            }

            if (postRepository.GetPostByID(postID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { status = "Post with given ID does not exist", content = "" });
            }

            var evaluations = evaluationRepository.GetEvaluationsByPostID(postID);

            if (evaluations.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { status = "This post has no evaluations added", content = "" });
            }

            return StatusCode(StatusCodes.Status200OK, new { status = "OK", content = evaluations });
        }

        /// <summary>
        /// Return evaluations with provided mark from the specified post 
        /// </summary>
        /// <returns>List of evaluations with specified mark for the selected post</returns>
        /// <remarks>
        /// GET 'https://localhost:32647/api/evaluations/byMark' \
        ///     Example of successful request \
        ///         --header 'Authorization: Bearer YWRtaW46c3VwZXJBZG1pbjEyMw==' \
        ///         --param  'postID = 1' \
        ///         --param  'mark = 5' \
        ///     Example of a failed request \
        ///         --header 'Authorization: Bearer YWRtaW46c3VwZXJBZG1pbjEyMw==' \
        ///         --param  'postID = 3' \
        ///         --param  'mark = 1' \
        /// </remarks>
        /// <param name="postID">Post ID</param>
        /// <param name="mark">Mark</param>
        /// <param name="key">Authorization header</param>
        /// <response code="200">Return list of evaluations with provided mark for given post</response>
        /// <response code="400">Post with provided ID does not exist or does not have any evaluations or evaluations with provided mark</response>
        /// <response code="401">Authorization error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("byMark")]
        public ActionResult<List<Evaluation>> GetEvaluationsOnPostByMark([FromHeader(Name = "Authorization")] string key, [FromQuery] int postID, [FromQuery] int mark)
        {
            if (!Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = ""
                });
            }

            if (postRepository.GetPostByID(postID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { status = "Post with given ID does not exist", content = "" });
            }

            if (evaluationRepository.GetEvaluationsByPostID(postID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { status = "This post has no evaluations", content = "" });
            }

            var evaluations = evaluationRepository.GetEvaluationsOnPostByMark(postID, mark);

            if (evaluations.Count == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { status = "This post has no evaluations with mark "+mark, content = "" });
            }

            return StatusCode(StatusCodes.Status200OK, new { status = "OK", content = evaluations });
        }

        /// <summary>
        /// Return all evaluations that account submited 
        /// </summary>
        /// <returns>List of evaluations from the specified Account</returns>
        /// <remarks>
        /// GET 'https://localhost:32647/api/evaluations/byAccountID' \
        ///     Example of successful request \
        ///         --header 'Authorization: Bearer YWRtaW46c3VwZXJBZG1pbjEyMw==' \
        ///         --param  'accountID = 2' \
        ///     Example of a failed request \
        ///         --header 'Authorization: Bearer YWRtaW46c3VwZXJBZG1pbjEyMw==' \
        ///         --param  'accountID = 5'
        /// </remarks>
        /// <param name="accountID">Account ID</param>
        /// <param name="key">Authorization header</param>
        /// <response code="200">Return list of evaluations for given account</response>
        /// <response code="400">Account with provided ID does not exist</response>
        /// <response code="401">Authorization error</response>
        /// <response code="404">There is still no posted evaluations for provided account ID</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("byAccountID")]
        public ActionResult<List<Evaluation>> GetEvaluationsByAccountID([FromHeader(Name = "Authorization")] string key, [FromQuery] int accountID)
        {
            if (!Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = ""
                });
            }

            if (accountRepository.GetAccountByID(accountID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { status = "Account with given ID does not exist", content = "" });
            }

            var evaluations = evaluationRepository.GetEvaluationsByAccountID(accountID);

            if (evaluations.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { status = "This account has no submited evaluations yet.", content = "" });
            }

            return StatusCode(StatusCodes.Status200OK, new { status = "OK", content = evaluations });
        }

        /// <summary>
        /// Submits new evaluation
        /// </summary>
        /// <param name="evaluationDto">Model of evaluation that is being submited</param>
        /// <param name="accountID">Account ID that sends request</param>
        /// <param name="key">Authorization header</param>
        /// <returns></returns>
        /// <remarks>
        /// POST 'https://localhost:49877/api/evaluations/' \
        /// Example of successful request \
        ///  --header 'Authorization: Bearer YWRtaW46c3VwZXJBZG1pbjEyMw==' \
        ///  --param 'accountID = 2' \
        ///  --body \
        /// {     \
        ///     "PostID": 1, \
        ///     "Mark": 5, \
        ///     "Description":"Test." \
        /// } \
        /// </remarks>
        /// <response code="201">Return confirmation that new evaluation is sumbited</response>
        /// <response code="400">Post with given ID does not exist</response>
        /// <response code="401">Authorization error</response>
        /// <response code="500">Server Error while submiting evaluation</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public IActionResult CreateEvaluation([FromHeader(Name = "Authorization")] string key, [FromBody] EvaluationCreateDto evaluationDto, [FromQuery] int accountID)
        {
            if (!Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = ""
                });
            }

            if (postRepository.GetPostByID(evaluationDto.PostID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { status = "Post with given ID does not exist", content = "" });
            }

            if (accountRepository.GetAccountByID(accountID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { status = "Account with given ID does not exist", content = "" });
            }

            if (evaluationRepository.CheckIfAccountAlredyEvaluatedPost(evaluationDto.PostID, accountID))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { status = "Account with given ID alredy evaluated this post", content = "" });
            }

            Account accVO = new Account(accountRepository.getUsernameByID(accountID));

            var newEvaluation = new Evaluation
            {
                Mark = evaluationDto.Mark,
                Description = evaluationDto.Description,
                AccountID = accountID,
                PostID = evaluationDto.PostID,
                Account = accVO
            };

            try
            {
                evaluationRepository.CreateEvaluation(newEvaluation);
                evaluationRepository.SaveChanges();
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully created new evaluation with ID {0} in database", newEvaluation.EvaluationID), null);

                return StatusCode(StatusCodes.Status201Created, new { status = "Evaluation is successfully submited!", content = newEvaluation });
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Evaluation with ID {0} not submited, message: {1}", newEvaluation.EvaluationID, ex.Message), null);

                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Create error, check logs for more info." + ex.Message, content = "" });
            }
        }

        /// <summary>
        /// Update evaluation with provied ID
        /// </summary>
        /// <param name="newEvaluation">Evaluation model that is going to be updated</param>
        /// <param name="key">Authorization header</param>
        /// <returns></returns>
        /// <remarks>
        /// PUT 'https://localhost:32647/api/evaluations' \
        /// Example of successful request    \
        ///  --header 'Authorization: Bearer YWRtaW46c3VwZXJBZG1pbjEyMw=='  \
        ///  --body \
        /// { \
        ///     "evaluationID": "1cc45ba4-bbb9-41ad-b8fa-c768a4f14ca5", \
        ///     "postID": 1, \
        ///     "content": "Updated succ!" \
        /// } 
        /// </remarks>
        /// <response code="200">Return confirmation that evaluation is updated</response>
        /// <response code="401">Authorization error</response>
        /// <response code="404">Evaluation with provied ID not found</response>
        /// <response code="500">Server Error while updating evaluation</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public IActionResult UpdateEvaluation([FromHeader(Name = "Authorization")] string key, [FromBody] EvaluationUpdateDto newEvaluation)
        {
            if (!Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = ""
                });
            }

            if (evaluationRepository.GetEvaluationByID(newEvaluation.EvaluationID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { status = "There is no evaluation with given ID!", content = "" });
            }

            var evaluationToUpdate = evaluationRepository.GetEvaluationByID(newEvaluation.EvaluationID);

            evaluationToUpdate.Mark = newEvaluation.Mark;
            evaluationToUpdate.Description = newEvaluation.Description;

            try
            {
                evaluationRepository.UpdateEvaluation(evaluationToUpdate);
                evaluationRepository.SaveChanges();
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully updated evaluation with ID {0} in database", evaluationToUpdate.EvaluationID), null);

                return StatusCode(StatusCodes.Status200OK, new { status = "Evaluation updated!", content = evaluationToUpdate });

            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Evaluation with ID {0} not updated, message: {1}", evaluationToUpdate.EvaluationID, ex.Message), null);

                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Update error, check logs for more info.", content = "" });

            }

        }

        /// <summary>
        /// Delete evaluation with provied ID
        /// </summary>
        /// <param name="evaluationID">Evaluation ID that is going to be removed</param>
        /// <param name="key">Authorization header</param>
        /// <remarks>        
        /// Example of successful request    \
        /// DELETE 'https://localhost:32647/api/evaluation' \
        ///     --header 'Authorization: Bearer YWRtaW46c3VwZXJBZG1pbjEyMw==' \
        ///     --param  'evaluationID = 40b090d8-9e0f-470b-9e0e-2df13e05e935'
        /// </remarks>
        /// <response code="200">Evaluation successfully deleted</response>
        /// <response code="401">Authorization error</response>
        /// <response code="404">Evaluation with provided ID does not exist</response>
        /// <response code="500">Server Error while deleting evaluation</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        public IActionResult DeleteEvaluation([FromHeader(Name = "Authorization")] string key, [FromQuery] Guid evaluationID)
        {
            if (!Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = ""
                });
            }

            var evaluation = evaluationRepository.GetEvaluationByID(evaluationID);
            if (evaluation == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { status = "Evaluation with provided id not found!", content = "" });
            }
            try
            {
                evaluationRepository.DeleteEvaluation(evaluationID);
                evaluationRepository.SaveChanges();
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully deleted evaluation with ID {0} from database", evaluationID), null);

                return StatusCode(StatusCodes.Status200OK, new { status = "Evaluation successfully deleted", content = "" });
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while deleting evaluation with ID {0}, message: {1}", evaluationID, ex.Message), null);
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Delete error, check logs for more info.", content = "" });
            }

        }
    }
}
