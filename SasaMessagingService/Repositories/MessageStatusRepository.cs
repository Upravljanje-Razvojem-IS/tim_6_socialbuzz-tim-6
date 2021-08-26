using AutoMapper;
using SasaMessagingService.CustomException;
using SasaMessagingService.Database;
using SasaMessagingService.DTOs.MessageStatusDtos;
using SasaMessagingService.Entities;
using SasaMessagingService.Interfaces;
using SasaMessagingService.LoggerMock;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SasaMessagingService.Repositories
{
    public class MessageStatusRepository : IMessageStatusRepository
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly FakeLogger _logger;

        public MessageStatusRepository(IMapper mapper, DatabaseContext context, FakeLogger logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        public MessageStatusConfirmationDto Create(MessageStatusPostDto dto)
        {
            MessageStatus newStatus = new MessageStatus()
            {
                Id = Guid.NewGuid(),
                Status = dto.Status
            };

            _context.MessageStatuses.Add(newStatus);
            _context.SaveChanges();

            _logger.Log("Message Status Created");

            return _mapper.Map<MessageStatusConfirmationDto>(newStatus);
        }

        public void Delete(Guid id)
        {
            var status = _context.MessageStatuses.FirstOrDefault(e => e.Id == id);

            if (status == null)
                throw new BussinessException("Message status does not exist");

            _context.MessageStatuses.Remove(status);
            _context.SaveChanges();

            _logger.Log("Message Status Deleted");

        }

        public List<MessageStatusGetDto> Get()
        {
            var list = _context.MessageStatuses.ToList();

            _logger.Log("Message Statuses Fetched");

            return _mapper.Map<List<MessageStatusGetDto>>(list);
        }

        public MessageStatusGetDto Get(Guid id)
        {
            var status = _context.MessageStatuses.FirstOrDefault(e => e.Id == id);

            _logger.Log("Message Status Fetched");

            return _mapper.Map<MessageStatusGetDto>(status);
        }

        public MessageStatusConfirmationDto Update(Guid id, MessageStatusPutDto dto)
        {
            var message = _context.MessageStatuses.FirstOrDefault(e => e.Id == id);

            if (message == null)
                throw new BussinessException("Message status does not exist");

            message.Status = dto.Status;

            _context.SaveChanges();

            _logger.Log("Message Status Updated");

            return _mapper.Map<MessageStatusConfirmationDto>(message);
        }
    }
}
