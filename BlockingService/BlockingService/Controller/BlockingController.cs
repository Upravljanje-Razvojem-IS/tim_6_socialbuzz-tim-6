using AutoMapper;
using BlockingService.Auth;
using BlockingService.Data.UnitOfWork;
using BlockingService.Exceptions;
using BlockingService.Logger;
using BlockingService.Model.Entity;
using BlockingService.Models.DTOs;
using BlockingService.Models.Mocks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;

namespace BlockingService.Controller
{
    [ApiController]
    [Route("api/block")]
    [Produces("application/json", "application/xml")]
    public class BlockingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authorization;
        private readonly IFakeLogger _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public BlockingController(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication authorization, IFakeLogger logger, IHttpContextAccessor contextAccessor)
        {
            _authorization = authorization;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }


        /// <summary>
        /// Return implemented options for API   
        /// </summary>
        /// <returns>Header key 'Allow' with allowed requests</returns>
        /// <remarks>
        /// Example of successful request \
        /// OPTIONS 'https://localhost:44304/api/block'
        /// </remarks>
        /// <response code="200">Return header key 'Allow' with allowed requests</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpOptions]
        public IActionResult GetBlockingOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, DELETE");
            return Ok();
        }


        /// <summary>
        /// User login mock - generates JWT
        /// </summary>
        /// <param name="loginDTO">Model of account</param>
        /// <remarks>
        /// POST 'https://localhost:44304/api/block/login/' \
        /// Example of a successful request \
        /// {
        /// "username": "jovana",
        /// "password": "jovanaa"
        ///  }
        /// </remarks>
        /// <response code="201">Returns generated JWT</response>
        /// <response code="400">Bad credidentials</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("login")]
        public ActionResult<String> Authenticate([FromBody] LoginDTO loginDTO)
        {
            Account account;
            try
            {
                account = _unitOfWork.RepositoryAccount.Exists(new Account(loginDTO.Username, loginDTO.Password));
            }
            catch (System.InvalidOperationException e)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Couldn't create JWT token because user: {0} doesn't exist. Error: {1}", loginDTO.Username, e.Message), null);
                return StatusCode(StatusCodes.Status400BadRequest, "User doesn't exist");
            }


            try
            {
                account.JWT = _authorization.GenerateToken(account.Username);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Couldn't create JWT token, message: {0} ", e.Message), null);
                return StatusCode(StatusCodes.Status400BadRequest, "Error while creating token, read more in logs");
            }

            _logger.Log(LogLevel.Information, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully generated JWT token for user: {0}", loginDTO.Username), null);
            return StatusCode(StatusCodes.Status201Created, new { status = "OK", content = account.JWT });

        }

        /// <summary>
        /// Return all blocks   
        /// </summary>
        /// <returns>List of all blocking relations</returns>
        /// <remarks>
        /// Example of successful request \
        /// GET 'https://localhost:44304/api/block' \
        /// </remarks>
        /// <response code="200">Return list of blocked accounts by all users</response>
        /// <response code="404">There is no blocked accounts</response>
        /// <response code="401">Authorization error</response>
        /// <response code="500">Server error while reading</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public ActionResult<List<BlockingDTO>> GetAll()
        {

            var blocks = _unitOfWork.RepositoryBlocking.GetAll();
            if (blocks == null || blocks.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, "There is no blocked accounts.");
            }
            try
            {
                return StatusCode(StatusCodes.Status200OK, new { status = "OK", content = blocks });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Reading error", content = "" });
            }
        }

        /// <summary>
        /// Return all blocked accounts   
        /// </summary>
        /// <param name="key">Authorization header</param>
        /// <returns>List of all blocked accounts for user</returns>
        /// <remarks>
        /// Example of successful request \
        /// GET 'https://localhost:44304/api/block/blocked' \
        ///     --header 'Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImpvdmFuYSIsIm5iZiI6MTYyODcxOTUzOCwiZXhwIjoxNjI4NzIzMTM4LCJpYXQiOjE2Mjg3MTk1Mzh9.327QAMSdiAmrOwQ-01Xk7kjXs4vOvbg86WbCLjFjtOk'
        /// </remarks>
        /// <response code="200">Return list of all blocked accounts for user</response>
        /// <response code="404">There is no blocked accounts</response>
        /// <response code="401">Authorization error</response>
        /// <response code="500">Server error while reading</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("blocked")]
        public ActionResult<List<BlockingDTO>> GetAllBlockedAccounts([FromHeader(Name = "Authorization")] string key)
        {
            String username;
            try
            {
                if (!_authorization.ValidateToken(key, out username))
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new
                    {
                        status = "Authorization failed",
                        content = ""
                    });
                }
            }
            catch (NullReferenceException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = e.Message
                });
            }

            var user = _unitOfWork.RepositoryAccount.GetAccountByUserName(username);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "User doesn't exist.");
            }
            if (user.JWT == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "JWT token expired. Log in again");
            }

            var blocks = _unitOfWork.RepositoryBlocking.GetAllBlockedAccounts(user);
            if (blocks == null || blocks.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, "There is no blocked accounts for this user.");
            }
            try
            {
                return StatusCode(StatusCodes.Status200OK, new { status = "OK", content = blocks });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Reading error", content = "" });
            }
        }

        /// <summary>
        /// Adds given user to accounts that current user blocks
        /// </summary>
        /// <param name="key">Authorization header</param>
        /// <param name="blockUserDTO">Model of account</param>
        /// <remarks>
        /// POST 'https://localhost:44304/api/block/blockUser/' \
        /// Example of a successful request \
        ///  --header 'Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImpvdmFuYSIsIm5iZiI6MTYyODcxOTUzOCwiZXhwIjoxNjI4NzIzMTM4LCJpYXQiOjE2Mjg3MTk1Mzh9.327QAMSdiAmrOwQ-01Xk7kjXs4vOvbg86WbCLjFjtOk'
        /// {
        /// "Blocked": "andrijana",
        ///  }
        /// </remarks>
        /// <response code="201">Returns the created blocking entity</response>
        /// <response code="400">Current user has already blocked given user</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">Given user account doesn't exist</response>
        /// <response code="500">There was an error on the server</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("blockUser")]
        public ActionResult<List<BlockingDTO>> Block([FromHeader(Name = "Authorization")] string key, [FromBody] BlockUserDTO blockUserDTO)
        {
            String username;
            try
            {
                if (!_authorization.ValidateToken(key, out username))
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new
                    {
                        status = "Authorization failed",
                        content = ""
                    });
                }
            }
            catch (NullReferenceException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = e.Message
                });
            }
            var blocker = _unitOfWork.RepositoryAccount.GetAccountByUserName(username);
            var blocked = _unitOfWork.RepositoryAccount.GetAccountByUserName(blockUserDTO.Blocked);
            if (blocked == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Blocked account doesn't exist.");
            }
            if (blocker.JWT == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "JWT token expired. Log in again");
            }

            if (_unitOfWork.RepositoryBlocking.Find(blocker, blocked))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Account has already been bloced by this account");
            }
            try
            {
                if (blocker.Account_id == blocked.Account_id)
                {
                    throw new SameAccountException("One account can't block itself");
                }
                Blocking f = _unitOfWork.RepositoryBlocking.Block(blocker, blocked);
                var block = _mapper.Map<BlockingDTO>(f);
                _unitOfWork.Commit();
                _logger.Log(LogLevel.Information, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully blocked user: {0} ", blocked.Username), null);
                return StatusCode(StatusCodes.Status201Created, new { status = "OK", content = block });
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while trying to block user: {0}, message: {1}", blocked.Username, e.Message), null);
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Insert error", content = e.Message });
            }
        }

        /// <summary>
        /// Deletes given user from accounts that current user has blocked
        /// </summary>
        /// <param name="blockUserDTO">Model of account</param>
        /// <param name="key">Authorization header</param>
        /// <remarks>
        /// POST 'https://localhost:44304/api/block/unblockUser/' \
        /// Example of a successful request \
        ///  --header 'Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImpvdmFuYSIsIm5iZiI6MTYyODcxOTUzOCwiZXhwIjoxNjI4NzIzMTM4LCJpYXQiOjE2Mjg3MTk1Mzh9.327QAMSdiAmrOwQ-01Xk7kjXs4vOvbg86WbCLjFjtOk'
        /// {
        /// "Blocked": "andrijana",
        ///  }
        /// </remarks>
        /// <response code="201">Returns the deleted blocked entity</response>
        /// <response code="400">Current user hasn't blocked given user</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">Given user account doesn't exist</response>
        /// <response code="500">There was an error on the server</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("unblockUser")]
        public ActionResult<List<BlockingDTO>> Unblock([FromHeader(Name = "Authorization")] string key, [FromBody] BlockUserDTO blockUserDTO)
        {
            String username;
            try
            {
                if (!_authorization.ValidateToken(key, out username))
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new
                    {
                        status = "Authorization failed",
                        content = ""
                    });
                }
            }
            catch (NullReferenceException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = e.Message
                });
            }catch(SecurityTokenExpiredException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = e.Message
                });
            }

            var blocker = _unitOfWork.RepositoryAccount.GetAccountByUserName(username);
            if (blocker == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Blocker account doesn't exist.");
            }
            var blocked = _unitOfWork.RepositoryAccount.GetAccountByUserName(blockUserDTO.Blocked);
            if (blocked == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Blocked account doesn't exist.");
            }
            if (blocker.JWT == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "JWT token expired. Log in again");
            }

            if (!_unitOfWork.RepositoryBlocking.Find(blocker, blocked))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Account has not been blocked by this account");
            }
            try
            {
                if (blocker.Account_id == blocked.Account_id)
                {
                    throw new SameAccountException("One account can't block itself");
                }
                Blocking f = _unitOfWork.RepositoryBlocking.Unblock(blocker, blocked);
                var block = _mapper.Map<BlockingDTO>(f);
                _unitOfWork.Commit();
                _logger.Log(LogLevel.Information, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully unblocked user: {0} ", blocked.Username), null);
                return StatusCode(StatusCodes.Status200OK, new { status = "OK", content = block });

            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while trying to unblock user: {0}, message: {1}", blocked.Username, e.Message), null);
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Delete error", content = "" });
            }
        }


    }
}
