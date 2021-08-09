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

namespace FollowingService.Controllers
{
    [ApiController]
    [Route("api/follow")]
    [Produces("application/json", "application/xml")]
    public class FollowingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FollowingController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpOptions]
        public IActionResult GetCommentsOpstions()
        {
            Response.Headers.Add("Allow", "GET, POST, DELETE");
            return Ok();
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public ActionResult<List<FollowingDTO>> GetAll()
        {
            var followings = _unitOfWork.RepositoryFollowing.GetAll();
            if (followings == null || followings.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, "There is no follows.");
            }
            try
            {
                return StatusCode(StatusCodes.Status200OK, new { status = "OK", content = followings });
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Reading error", content = "" });
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("followers/{username}")]
        public ActionResult<List<FollowingDTO>> GetAllFollowers(string username)
        {
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
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Reading error", content = "" });
            }
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("following/{username}")]
        public ActionResult<List<FollowingDTO>> GetAllFollowing(string username)
        {
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
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Reading error", content = "" });
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("followUser")]
        public ActionResult<List<FollowingDTO>> Follow([FromBody] FollowUserDTO followUserDTO)
        {
            var follower = _unitOfWork.RepositoryAccount.GetAccountByUserName(followUserDTO.Follower);
            if (follower == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Follower account doesn't exist.");
            }
            var following = _unitOfWork.RepositoryAccount.GetAccountByUserName(followUserDTO.Following);
            if (following == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Following account doesn't exist.");
            }

            if(_unitOfWork.RepositoryFollowing.Find(follower, following))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Following account has already been followed by following account");
            }
            if(follower.Account_id == following.Account_id)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "One account can't follow itself");
            }

            try
            {
            Following f = _unitOfWork.RepositoryFollowing.Follow(follower, following);
            var follow = _mapper.Map<FollowingDTO>(f);
            _unitOfWork.Commit();
           
            return StatusCode(StatusCodes.Status200OK, new { status = "OK", content = follow });
            }catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Insert error", content = "" });
            }
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("unfollowUser")]
        public ActionResult<List<FollowingDTO>> Unfollow([FromBody] FollowUserDTO followUserDTO)
        {
            var follower = _unitOfWork.RepositoryAccount.GetAccountByUserName(followUserDTO.Follower);
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
            if (follower.Account_id == following.Account_id)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "One account can't unfollow itself");
            }

            try
            {
            Following f = _unitOfWork.RepositoryFollowing.Unfollow(follower, following);
            var follow = _mapper.Map<FollowingDTO>(f);
            _unitOfWork.Commit();

            return StatusCode(StatusCodes.Status200OK, new { status = "OK", content = follow });

            }catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Delete error", content = "" });
            }
        }




    }
}
