using SasaMessagingService.DTOs.MessageDtos;
using System;
using System.Collections.Generic;

namespace SasaMessagingService.Interfaces
{
    public interface IMessageRepository
    {
        List<MessageGetDto> Get();
        MessageGetDto Get(Guid id);
        MessageConfirmationDto Create(MessagePostDto dto);
        MessageConfirmationDto Update(Guid id, MessagePutDto dto);
        void Delete(Guid id);
    }
}
