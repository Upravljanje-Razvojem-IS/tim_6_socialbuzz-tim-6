using System;
using System.Collections.Generic;
using UserService.DTOs.RoleDtos;

namespace UserService.Interfaces
{
    public interface IRoleRepository
    {
        List<RoleReadDto> Get();
        RoleReadDto Get(Guid id);
        RoleConfirmationDto Create(RolePostDto dto);
        RoleConfirmationDto Update(Guid id, RolePutDto dto);
        void Delete(Guid id);
    }
}
