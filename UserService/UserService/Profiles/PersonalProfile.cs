using AutoMapper;
using UserService.DTOs.PersonalDtos;
using UserService.Entities;

namespace UserService.Profiles
{
    public class PersonalProfile : Profile
    {
        public PersonalProfile()
        {
            CreateMap<Personal, PersonalReadDto>();
            CreateMap<Personal, PersonalConfirmationDto>();
        }
    }
}
