using AutoMapper;
using ReactionService.Entities;
using ReactionService.Models.DTOs.Reactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Profiles
{
    public class ReactionProfile : Profile
    {
        public ReactionProfile()
        {
            CreateMap<ReactionCreationDto, Reaction>();
        }
    }
}
