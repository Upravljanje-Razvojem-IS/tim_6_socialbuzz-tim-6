using AutoMapper;
using SasaMessagingService.DTOs.MessageStatusDtos;
using SasaMessagingService.Entities;

namespace SasaMessagingService.Profiles
{
    public class MessageTypeProfile : Profile
    {
        public MessageTypeProfile()
        {
            CreateMap<MessageStatus, MessageStatusGetDto>();
            CreateMap<MessageStatus, MessageStatusConfirmationDto>();
        }
    }
}
