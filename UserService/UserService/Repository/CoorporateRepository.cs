using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using UserService.Data;
using UserService.DTOs.CoorporateDtos;
using UserService.Entities;
using UserService.Interfaces;
using UserService.Logger;
using UserService.UserExcept;

namespace UserService.Repository
{
    public class CoorporateRepository : ICoorporateRepository
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly MockLogger _logger;

        public CoorporateRepository(IMapper mapper, DatabaseContext context, MockLogger logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }


        public CoorporateConfirmationDto Create(CoorporatePostDto dto)
        {
            Coorporate newCoorporate = new Coorporate()
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                Name = dto.Name,
                Surname = dto.Surname,
                Password = dto.Password,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                CoorporationName = dto.CoorporationName,
                RoleId = dto.RoleId
            };

            _context.Coorporates.Add(newCoorporate);

            _context.SaveChanges();

            _logger.Log("Create Coorporation account!");

            return _mapper.Map<CoorporateConfirmationDto>(newCoorporate);
        }

        public List<CoorporateReadDto> Get()
        {
            var list = _context.Coorporates.ToList();

            _logger.Log("Get Coorporation accounts!");

            return _mapper.Map<List<CoorporateReadDto>>(list);
        }

        public CoorporateReadDto Get(Guid id)
        {
            var entity = _context.Coorporates.FirstOrDefault(e => e.Id == id);

            _logger.Log("Get Coorporation account!");

            return _mapper.Map<CoorporateReadDto>(entity);
        }

        public CoorporateConfirmationDto Update(Guid id, CoorporatePutDto dto)
        {
            var coorporate = _context.Coorporates.FirstOrDefault(e => e.Id == id);

            if (coorporate == null)
                throw new UserException("User does not exist");

            coorporate.Username = dto.Username;
            coorporate.Name = dto.Name;
            coorporate.Surname = dto.Surname;
            coorporate.Password = dto.Password;
            coorporate.Email = dto.Email;
            coorporate.PhoneNumber = dto.PhoneNumber;
            coorporate.Address = dto.Address;
            coorporate.CoorporationName = dto.CoorporationName;
            coorporate.RoleId = dto.RoleId;

            _context.SaveChanges();

            _logger.Log("Update Coorporation account!");

            return _mapper.Map<CoorporateConfirmationDto>(coorporate);
        }

        public void Delete(Guid id)
        {
            var coorporate = _context.Coorporates.FirstOrDefault(e => e.Id == id);

            if (coorporate == null)
                throw new UserException("User does not exist");

            _context.Coorporates.Remove(coorporate);

            _logger.Log("Delete Coorporation account!");

            _context.SaveChanges();
        }

    }
}
