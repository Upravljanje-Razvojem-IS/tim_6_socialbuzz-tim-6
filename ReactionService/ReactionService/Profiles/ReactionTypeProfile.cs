using AutoMapper;
using ReactionService.Entities;
using ReactionService.Models.DTOs.ReactionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Profiles
{
    public class ReactionTypeProfile : Profile
    {
        public ReactionTypeProfile()
        {
            CreateMap<ReactionTypeCreationDto, ReactionType>();
            CreateMap<ReactionType, ReactionTypeConfirmationDto>();
        }
    }
}
