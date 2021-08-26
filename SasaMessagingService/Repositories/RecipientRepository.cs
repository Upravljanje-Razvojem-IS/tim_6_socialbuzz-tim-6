using AutoMapper;
using SasaMessagingService.CustomException;
using SasaMessagingService.Database;
using SasaMessagingService.DTOs.RecipientDtos;
using SasaMessagingService.Entities;
using SasaMessagingService.Interfaces;
using SasaMessagingService.LoggerMock;
using SasaMessagingService.MockData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SasaMessagingService.Repositories
{
    public class RecipientRepository : IRecipientRepository
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly FakeLogger _logger;

        public RecipientRepository(IMapper mapper, DatabaseContext context, FakeLogger logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        public RecipientConfirmationDto Create(RecipientPostDto dto)
        {
            var account = UserData.GetUsers().FirstOrDefault(e => e.Id == dto.AccountId);

            if (account == null)
                throw new BussinessException("Account does not exist");

            Recipient newRecipient = new Recipient()
            {
                Id = Guid.NewGuid(),
                MessageId = dto.MessageId,
                AccountId = dto.AccountId
            };

            _context.Recipients.Add(newRecipient);
            _context.SaveChanges();

            _logger.Log("Recipient Created");

            return _mapper.Map<RecipientConfirmationDto>(newRecipient);
        }

        public void Delete(Guid id)
        {
            var recipiet = _context.Recipients.FirstOrDefault(e => e.Id == id);

            if (recipiet == null)
                throw new BussinessException("Recipiet does not exist");

            _context.Recipients.Remove(recipiet);
            _context.SaveChanges();

            _logger.Log("Recipient Deleted");

        }

        public List<RecipientGetDto> Get()
        {
            var list = _context.Recipients.ToList();

            return _mapper.Map<List<RecipientGetDto>>(list);

            _logger.Log("Recipients Fetched");

        }

        public RecipientGetDto Get(Guid id)
        {
            var recipiet = _context.Recipients.FirstOrDefault(e => e.Id == id);

            _logger.Log("Recipient Fetched");

            return _mapper.Map<RecipientGetDto>(recipiet);
        }

        public RecipientConfirmationDto Update(Guid id, RecipientPutDto dto)
        {
            var recipiet = _context.Recipients.FirstOrDefault(e => e.Id == id);

            if (recipiet == null)
                throw new BussinessException("Recipiet does not exist");

            var account = UserData.GetUsers().FirstOrDefault(e => e.Id == dto.AccountId);

            if (account == null)
                throw new BussinessException("Account does not exist");

            recipiet.AccountId = dto.AccountId;
            recipiet.MessageId = dto.MessageId;

            _context.SaveChanges();

            _logger.Log("Recipient Updated");

            return _mapper.Map<RecipientConfirmationDto>(recipiet);
        }
    }
}
