using SasaMessagingService.DTOs.MessageStatusDtos;
using System;
using System.Collections.Generic;

namespace SasaMessagingService.Interfaces
{
    public interface IMessageStatusRepository
    {
        List<MessageStatusGetDto> Get();
        MessageStatusGetDto Get(Guid id);
        MessageStatusConfirmationDto Create(MessageStatusPostDto dto);
        MessageStatusConfirmationDto Update(Guid id, MessageStatusPutDto dto);
        void Delete(Guid id);
    }
}
