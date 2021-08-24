﻿using System;

namespace UserService.DTOs.PersonalDtos
{
    public class PersonalPostDto
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public Guid RoleId { get; set; }
    }
}
