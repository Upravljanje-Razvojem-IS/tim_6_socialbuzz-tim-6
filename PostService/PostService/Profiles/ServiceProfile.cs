using AutoMapper;
using PostService.Entities.Posts;
using PostService.Models.DTOs.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Profiles
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<Service, PostDto>().ForMember(
                dest => dest.PostType,
                opt => opt.MapFrom(src => "Usluga"));

            CreateMap<Service, Post>();
        }
    }
}
