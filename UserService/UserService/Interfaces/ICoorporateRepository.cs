using System;
using System.Collections.Generic;
using UserService.DTOs.CoorporateDtos;

namespace UserService.Interfaces
{
    public interface ICoorporateRepository
    {
        List<CoorporateReadDto> Get();
        CoorporateReadDto Get(Guid id);
        CoorporateConfirmationDto Create(CoorporatePostDto dto);
        CoorporateConfirmationDto Update(Guid id, CoorporatePutDto dto);
        void Delete(Guid id);
    }
}
