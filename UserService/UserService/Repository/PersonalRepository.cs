using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using UserService.Data;
using UserService.DTOs.PersonalDtos;
using UserService.Entities;
using UserService.Interfaces;
using UserService.Logger;
using UserService.UserExcept;

namespace UserService.Repository
{
    public class PersonalRepository : IPersonalRepository
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly MockLogger _logger;

        public PersonalRepository(IMapper mapper, DatabaseContext context, MockLogger logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        public PersonalConfirmationDto Create(PersonalPostDto dto)
        {
            Personal newPersonal = new Personal()
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                Name = dto.Name,
                Surname = dto.Surname,
                Password = dto.Password,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                RoleId = dto.RoleId
            };

            _context.Personals.Add(newPersonal);

            _context.SaveChanges();

            _logger.Log("Create Personal account!");

            return _mapper.Map<PersonalConfirmationDto>(newPersonal);
        }

        public List<PersonalReadDto> Get()
        {
            var list = _context.Personals.ToList();

            _logger.Log("Get Personal accounts!");

            return _mapper.Map<List<PersonalReadDto>>(list);
        }

        public PersonalReadDto Get(Guid id)
        {
            var entity = _context.Personals.FirstOrDefault(e => e.Id == id);

            _logger.Log("Get Personal account!");

            return _mapper.Map<PersonalReadDto>(entity);
        }

        public PersonalConfirmationDto Update(Guid id, PersonalPutDto dto)
        {
            var personal = _context.Personals.FirstOrDefault(e => e.Id == id);

            if (personal == null)
                throw new UserException("User does not exist");

            personal.Username = dto.Username;
            personal.Name = dto.Name;
            personal.Surname = dto.Surname;
            personal.Password = dto.Password;
            personal.Email = dto.Email;
            personal.PhoneNumber = dto.PhoneNumber;
            personal.Address = dto.Address;
            personal.RoleId = dto.RoleId;

            _context.SaveChanges();

            _logger.Log("Update Personal account!");

            return _mapper.Map<PersonalConfirmationDto>(personal);
        }

        public void Delete(Guid id)
        {
            var personal = _context.Personals.FirstOrDefault(e => e.Id == id);

            if (personal == null)
                throw new UserException("User does not exist");

            _context.Personals.Remove(personal);

            _logger.Log("Delete Personal account!");

            _context.SaveChanges();
        }

    }
}
