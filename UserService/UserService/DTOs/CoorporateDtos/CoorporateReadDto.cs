using System;
using UserService.Entities;

namespace UserService.DTOs.CoorporateDtos
{
    public class CoorporateReadDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string CoorporationName { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

    }
}
