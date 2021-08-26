using AutoMapper;
using SasaMessagingService.DTOs.MessageDtos;
using SasaMessagingService.Entities;

namespace SasaMessagingService.Profiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, MessageGetDto>();
            CreateMap<Message, MessageConfirmationDto>();
        }
    }
}
