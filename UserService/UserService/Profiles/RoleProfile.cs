using AutoMapper;
using UserService.DTOs.RoleDtos;
using UserService.Entities;

namespace UserService.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleReadDto>();
            CreateMap<Role, RoleConfirmationDto>();
        }
    }
}
