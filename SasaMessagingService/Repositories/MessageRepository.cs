using AutoMapper;
using SasaMessagingService.CustomException;
using SasaMessagingService.Database;
using SasaMessagingService.DTOs.MessageDtos;
using SasaMessagingService.Entities;
using SasaMessagingService.Interfaces;
using SasaMessagingService.LoggerMock;
using SasaMessagingService.MockData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SasaMessagingService.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly FakeLogger _logger;

        public MessageRepository(DatabaseContext context, IMapper mapper, FakeLogger logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public MessageConfirmationDto Create(MessagePostDto dto)
        {
            var seller = UserData.GetUsers().FirstOrDefault(e => e.Id == dto.SenderId);

            if (seller == null)
                throw new BussinessException("Seller does not exist");

            Message newMessage = new Message()
            {
                Id = Guid.NewGuid(),
                Content = dto.Content,
                Date = dto.Date,
                SenderId = dto.SenderId,
                StatusId = dto.StatusId
            };

            _context.Messages.Add(newMessage);
            _context.SaveChanges();

            _logger.Log("Message Created");

            return _mapper.Map<MessageConfirmationDto>(newMessage);
        }

        public void Delete(Guid id)
        {
            var message = _context.Messages.FirstOrDefault(e => e.Id == id);

            if(message == null)
                throw new BussinessException("Message does not exist");

            _context.Messages.Remove(message);
            _context.SaveChanges();

            _logger.Log("Message Deleted");
        }

        public List<MessageGetDto> Get()
        {
            var list = _context.Messages.ToList();

            _logger.Log("Messages fetched");

            return _mapper.Map<List<MessageGetDto>>(list);
        }

        public MessageGetDto Get(Guid id)
        {
            var message = _context.Messages.FirstOrDefault(e => e.Id == id);

            _logger.Log("Message fetched");

            return _mapper.Map<MessageGetDto>(message);
        }

        public MessageConfirmationDto Update(Guid id, MessagePutDto dto)
        {
            var message = _context.Messages.FirstOrDefault(e => e.Id == id);

            if (message == null)
                throw new BussinessException("Message does not exist");

            var seller = UserData.GetUsers().FirstOrDefault(e => e.Id == dto.SenderId);

            if (seller == null)
                throw new BussinessException("Seller does not exist");

            message.Content = dto.Content;
            message.Date = dto.Date;
            message.StatusId = dto.StatusId;
            message.SenderId = dto.SenderId;

            _context.SaveChanges();

            _logger.Log("Message Updated");

            return _mapper.Map<MessageConfirmationDto>(message);
        }
    }
}
