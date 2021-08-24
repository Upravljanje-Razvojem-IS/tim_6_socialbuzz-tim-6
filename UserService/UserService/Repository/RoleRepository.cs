using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using UserService.Data;
using UserService.DTOs.RoleDtos;
using UserService.Entities;
using UserService.Interfaces;
using UserService.Logger;
using UserService.UserExcept;

namespace UserService.Repository
{
    public class RoleRepository : IRoleRepository
    {

        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly MockLogger _logger;


        public RoleRepository(DatabaseContext context, IMapper mapper, MockLogger logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        public RoleConfirmationDto Create(RolePostDto dto)
        {
            Role newRole = new Role()
            {
                Id = Guid.NewGuid(),
                RoleName = dto.RoleName
            };

            _context.Roles.Add(newRole);

            _context.SaveChanges();

            _logger.Log("Create Role account!");

            return _mapper.Map<RoleConfirmationDto>(newRole);
        }

        public List<RoleReadDto> Get()
        {
            var list = _context.Roles.ToList();

            _logger.Log("Get Role accounts!");

            return _mapper.Map<List<RoleReadDto>>(list);
        }

        public RoleReadDto Get(Guid id)
        {
            var entity = _context.Roles.FirstOrDefault(e => e.Id == id);

            _logger.Log("Get Role account!");

            return _mapper.Map<RoleReadDto>(entity);
        }

        public RoleConfirmationDto Update(Guid id, RolePutDto dto)
        {
            var role = _context.Roles.FirstOrDefault(e => e.Id == id);

            if (role == null)
                throw new UserException("Role does not exist");

            role.RoleName = dto.RoleName;

            _context.SaveChanges();

            _logger.Log("Update Role account!");

            return _mapper.Map<RoleConfirmationDto>(role);
        }

        public void Delete(Guid id)
        {
            var role = _context.Roles.FirstOrDefault(e => e.Id == id);

            if (role == null)
                throw new UserException("Role does not exist");

            _context.Roles.Remove(role);

            _logger.Log("Delete Role account!");

            _context.SaveChanges();
        }

    }
}
