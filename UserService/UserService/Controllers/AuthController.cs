using Microsoft.AspNetCore.Mvc;
using UserService.Interfaces;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Login coorporate
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>token</returns>
        [HttpPost("coorporate")]
        public ActionResult LoginCoorporate([FromBody] Principal principal)
        {
            string token = _authService.LoginCoorporate(principal);

            return Ok(new { Token = token });
        }

        /// <summary>
        /// Login Personal
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>token</returns>
        [HttpPost("personal")]
        public ActionResult LoginPersonal([FromBody] Principal principal)
        {
            string token = _authService.LoginPersonal(principal);

            return Ok(new { Token = token });
        }
    }
}
