using AutoMapper;
using FollowingService.Data.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FollowingService.Models.DTOs;
using FollowingService.Data.AccountMock;
using FollowingService.Model.Entity;
using FollowingService.Exceptions;
using FollowingService.Auth;
using Microsoft.Extensions.Options;
using FollowingService.Models.Mocks;
using Microsoft.Extensions.Logging;
using FollowingService.Logger;

namespace FollowingService.Controllers
{
    [ApiController]
    [Route("api/follow")]
    [Produces("application/json", "application/xml")]
    public class FollowingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthorization _authorization;
        private readonly IFakeLogger _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public FollowingController(IUnitOfWork unitOfWork, IMapper mapper, IAuthorization authorization, IFakeLogger logger, IHttpContextAccessor contextAccessor)
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
        /// OPTIONS 'https://localhost:44372/api/follow'
        /// </remarks>
        /// <response code="200">Return header key 'Allow' with allowed requests</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpOptions]
        public IActionResult GetFollowingOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, DELETE");
            return Ok();
        }

        /// <summary>
        /// User login mock - generates JWT
        /// </summary>
        /// <param name="LoginDTO">Model of account</param>
        /// <remarks>
        /// POST 'https://localhost:44372/api/follow/login/' \
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
        public ActionResult<String> Authenticate([FromBody] LoginDTO acc)
        {
            Account account = _unitOfWork.RepositoryAccount.Exists(new Account(acc.Username, acc.Password));
            if (account != null)
            {
                try
                {
                    account.JWT = _authorization.GenerateToken(account.Username);
                }
                catch(Exception e)
                {
                    _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Couldn't create JWT token, message: {0} ", e.Message), null);
                    return StatusCode(StatusCodes.Status400BadRequest, "Error while creating token, read more in logs");
                }
             
                _logger.Log(LogLevel.Information, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully generated JWT token for user: {0}", acc.Username), null);
                return StatusCode(StatusCodes.Status201Created, new { status = "OK", content = account.JWT });
            }
            else
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Couldn't create JWT token because user: {0} doesn't exist", acc.Username), null);
                return StatusCode(StatusCodes.Status400BadRequest, "User doesn't exist");
            }
        }


        /// <summary>
        /// Return all followings   
        /// </summary>
        /// <param name="key">Authorization header</param>
        /// <returns>List of all following relations</returns>
        /// <remarks>
        /// Example of successful request \
        /// GET 'https://localhost:44372/api/follow' \
        ///     --header 'Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImpvdmFuYSIsIm5iZiI6MTYyODcxOTUzOCwiZXhwIjoxNjI4NzIzMTM4LCJpYXQiOjE2Mjg3MTk1Mzh9.327QAMSdiAmrOwQ-01Xk7kjXs4vOvbg86WbCLjFjtOk'
        /// </remarks>
        /// <response code="200">Return list of followings</response>
        /// <response code="404">There is no follows</response>
        /// <response code="401">Authorization error</response>
        /// <response code="500">Server error while reading</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public ActionResult<List<FollowingDTO>> GetAll([FromHeader(Name = "Authorization")] string key)
        {
            String username;
            if (!_authorization.ValidateToken(key, out username))
            {     
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = ""
                });
            }

            var followings = _unitOfWork.RepositoryFollowing.GetAll();
            if (followings == null || followings.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, "There is no follows.");
            }
            try
            {
                return StatusCode(StatusCodes.Status200OK, new { status = "OK", content = followings });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Reading error", content = "" });
            }
        }


        /// <summary>
        /// Return all followers   
        /// </summary>
        /// <param name="key">Authorization header</param>
        /// <returns>List of all followers for user</returns>
        /// <remarks>
        /// Example of successful request \
        /// GET 'https://localhost:44372/api/follow/followers' \
        ///     --header 'Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImpvdmFuYSIsIm5iZiI6MTYyODcxOTUzOCwiZXhwIjoxNjI4NzIzMTM4LCJpYXQiOjE2Mjg3MTk1Mzh9.327QAMSdiAmrOwQ-01Xk7kjXs4vOvbg86WbCLjFjtOk'
        /// </remarks>
        /// <response code="200">Return list of all followers for user</response>
        /// <response code="404">There is no follows</response>
        /// <response code="401">Authorization error</response>
        /// <response code="500">Server error while reading</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("followers")]
        public ActionResult<List<FollowingDTO>> GetAllFollowers([FromHeader(Name = "Authorization")] string key)
        {
            String username;
            if (!_authorization.ValidateToken(key, out username))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = ""
                });
            }
            var user = _unitOfWork.RepositoryAccount.GetAccountByUserName(username);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "User doesn't exist.");
            }

            var followings = _unitOfWork.RepositoryFollowing.GetAllFollowers(user);
            if (followings == null || followings.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, "There is no followers for this user.");
            }
            try
            {
                return StatusCode(StatusCodes.Status200OK, new { status = "OK", content = followings });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Reading error", content = "" });
            }
        }

        /// <summary>
        /// Return all accounts that user follows   
        /// </summary>
        /// <param name="key">Authorization header</param>
        /// <returns>List of all followers for user</returns>
        /// <remarks>
        /// Example of successful request \
        /// GET 'https://localhost:44372/api/follow/followings' \
        ///     --header 'Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImpvdmFuYSIsIm5iZiI6MTYyODcxOTUzOCwiZXhwIjoxNjI4NzIzMTM4LCJpYXQiOjE2Mjg3MTk1Mzh9.327QAMSdiAmrOwQ-01Xk7kjXs4vOvbg86WbCLjFjtOk'
        /// </remarks>
        /// <response code="200">Return list of all followers for user</response>
        /// <response code="404">There is no follows</response>
        /// <response code="401">Authorization error</response>
        /// <response code="500">Server error while reading</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("following")]
        public ActionResult<List<FollowingDTO>> GetAllFollowing([FromHeader(Name = "Authorization")] string key)
        {
            String username;
            if (!_authorization.ValidateToken(key, out username))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = ""
                });
            }
            var user = _unitOfWork.RepositoryAccount.GetAccountByUserName(username);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "User doesn't exist.");
            }

            var followings = _unitOfWork.RepositoryFollowing.GetAllFollowing(user);
            if (followings == null || followings.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, "There is no following for this user.");
            }
            try
            {
                return StatusCode(StatusCodes.Status200OK, new { status = "OK", content = followings });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Reading error", content = "" });
            }
        }



        /// <summary>
        /// Adds given user to accounts that current user follows
        /// </summary>
        /// <param name="followUserDTO">Model of account</param>
        /// <remarks>
        /// POST 'https://localhost:44372/api/follow/followUser/' \
        /// Example of a successful request \
        ///  --header 'Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImpvdmFuYSIsIm5iZiI6MTYyODcxOTUzOCwiZXhwIjoxNjI4NzIzMTM4LCJpYXQiOjE2Mjg3MTk1Mzh9.327QAMSdiAmrOwQ-01Xk7kjXs4vOvbg86WbCLjFjtOk'
        /// {
        /// "Following": "andrijana",
        ///  }
        /// </remarks>
        /// <response code="201">Returns the created following entity</response>
        /// <response code="400">Current user already follow given user</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">Given user account doesn't exist</response>
        /// <response code="500">There was an error on the server</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("followUser")]
        public ActionResult<List<FollowingDTO>> Follow([FromHeader(Name = "Authorization")] string key, [FromBody] FollowUserDTO followUserDTO)
        {
            String username;
            if (!_authorization.ValidateToken(key, out username))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = ""
                });
            }

            var follower = _unitOfWork.RepositoryAccount.GetAccountByUserName(username);   
            var following = _unitOfWork.RepositoryAccount.GetAccountByUserName(followUserDTO.Following);
            if (following == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Following account doesn't exist.");
            }

            if (_unitOfWork.RepositoryFollowing.Find(follower, following))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Following account has already been followed by following account");
            }
            try
            {
                if (follower.Account_id == following.Account_id)
                {
                    throw new SameAccountException("One account can't unfollow itself");
                }
                Following f = _unitOfWork.RepositoryFollowing.Follow(follower, following);
                var follow = _mapper.Map<FollowingDTO>(f);
                _unitOfWork.Commit();
                _logger.Log(LogLevel.Information, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully followed user: {0} ", following.Username), null);
                return StatusCode(StatusCodes.Status201Created, new { status = "OK", content = follow });
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while trying to follow user: {0}, message: {1}", following.Username, e.Message), null);
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Insert error", content = "" });
            }
        }


        /// <summary>
        /// Deletes given user from accounts that current user follows
        /// </summary>
        /// <param name="followUserDTO">Model of account</param>
        /// <remarks>
        /// POST 'https://localhost:44372/api/follow/unfollowUser/' \
        /// Example of a successful request \
        ///  --header 'Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImpvdmFuYSIsIm5iZiI6MTYyODcxOTUzOCwiZXhwIjoxNjI4NzIzMTM4LCJpYXQiOjE2Mjg3MTk1Mzh9.327QAMSdiAmrOwQ-01Xk7kjXs4vOvbg86WbCLjFjtOk'
        /// {
        /// "Following": "andrijana",
        ///  }
        /// </remarks>
        /// <response code="201">Returns the deleted following entity</response>
        /// <response code="400">Current user doesn't follow given user</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">Given user account doesn't exist</response>
        /// <response code="500">There was an error on the server</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("unfollowUser")]
        public ActionResult<List<FollowingDTO>> Unfollow([FromHeader(Name = "Authorization")] string key, [FromBody] FollowUserDTO followUserDTO)
        {
            String username;
            if (!_authorization.ValidateToken(key, out username))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = ""
                });
            }
            var follower = _unitOfWork.RepositoryAccount.GetAccountByUserName(username);
            if (follower == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Follower account doesn't exist.");
            }
            var following = _unitOfWork.RepositoryAccount.GetAccountByUserName(followUserDTO.Following);
            if (following == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Following account doesn't exist.");
            }

            if (!_unitOfWork.RepositoryFollowing.Find(follower, following))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Following account is not beeing followed by follower account");
            }
            try
            {
                if (follower.Account_id == following.Account_id)
                {
                    throw new SameAccountException("One account can't unfollow itself");
                }
                Following f = _unitOfWork.RepositoryFollowing.Unfollow(follower, following);
                var follow = _mapper.Map<FollowingDTO>(f);
                _unitOfWork.Commit();
                _logger.Log(LogLevel.Information, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully unfollowed user: {0} ", following.Username), null);
                return StatusCode(StatusCodes.Status200OK, new { status = "OK", content = follow });

            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, _contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while trying to unfollow user: {0}, message: {1}", following.Username, e.Message), null);
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Delete error", content = "" });
            }
        }




    }
}
