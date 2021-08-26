using AutoMapper;
using SasaMessagingService.DTOs.RecipientDtos;
using SasaMessagingService.Entities;

namespace SasaMessagingService.Profiles
{
    public class RecepientProfile : Profile
    {
        public RecepientProfile()
        {
            CreateMap<Recipient, RecipientGetDto>();
            CreateMap<Recipient, RecipientConfirmationDto>();
        }
    }
}
