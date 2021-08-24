using AutoMapper;
using UserService.DTOs.CoorporateDtos;
using UserService.Entities;

namespace UserService.Profiles
{
    public class CoorporateProfile : Profile
    {
        public CoorporateProfile()
        {
            CreateMap<Coorporate, CoorporateReadDto>();
            CreateMap<Coorporate, CoorporateConfirmationDto>();
        }
    }
}
