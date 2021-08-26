using SasaMessagingService.DTOs.RecipientDtos;
using System;
using System.Collections.Generic;

namespace SasaMessagingService.Interfaces
{
    public interface IRecipientRepository
    {
        List<RecipientGetDto> Get();
        RecipientGetDto Get(Guid id);
        RecipientConfirmationDto Create(RecipientPostDto dto);
        RecipientConfirmationDto Update(Guid id, RecipientPutDto dto);
        void Delete(Guid id);
    }
}
