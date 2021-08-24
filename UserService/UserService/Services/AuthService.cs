using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using UserService.Data;
using UserService.Interfaces;
using UserService.Models;
using UserService.UserExcept;

namespace UserService.Services
{
    public class AuthService : IAuthService
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _config;

        public AuthService(IConfiguration config, DatabaseContext context)
        {
            _config = config;
            _context = context;
        }

        public string LoginCoorporate(Principal principal)
        {
            var coorporate = _context.Coorporates.FirstOrDefault(e => e.Email == principal.Email && e.Password == principal.Password);

            if (coorporate == null)
                throw new UserException("User does not exist");

            return this.GenerateJwt("Coorporate");
        }

        public string LoginPersonal(Principal principal)
        {
            var personal = _context.Personals.FirstOrDefault(e => e.Email == principal.Email && e.Password == principal.Password);

            if (personal == null)
                throw new UserException("User does not exist");

            return this.GenerateJwt("Personal");
        }

        public string GenerateJwt(string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                                             _config["Jwt:Issuer"],
                                             new[] { new Claim("role", role) },
                                             expires: DateTime.Now.AddMinutes(120),
                                             signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
