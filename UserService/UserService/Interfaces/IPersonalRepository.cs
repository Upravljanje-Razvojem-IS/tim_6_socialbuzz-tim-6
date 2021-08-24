using System;
using System.Collections.Generic;
using UserService.DTOs.PersonalDtos;

namespace UserService.Interfaces
{
    public interface IPersonalRepository
    {
        List<PersonalReadDto> Get();
        PersonalReadDto Get(Guid id);
        PersonalConfirmationDto Create(PersonalPostDto dto);
        PersonalConfirmationDto Update(Guid id, PersonalPutDto dto);
        void Delete(Guid id);
    }
}
