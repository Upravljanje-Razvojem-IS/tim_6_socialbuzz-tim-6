using AutoMapper;
using PostService.Entities;
using PostService.Models.DTOs.PostHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Profiles
{
    public class PostHistoryProfile : Profile
    {
        public PostHistoryProfile()
        {
            CreateMap<PostHistoryCreationDto, PostHistory>();
            CreateMap<PostHistoryUpdateDto, PostHistory>();
        }
    }
}
