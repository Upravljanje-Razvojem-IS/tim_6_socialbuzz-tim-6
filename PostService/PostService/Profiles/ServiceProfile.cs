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
            CreateMap<ServiceCreationDto, Service>();
            CreateMap<Service, ServiceConfirmationDto>().ForMember(
                    dest => dest.Price,
                    opt => opt.MapFrom(src => $"{ src.Price } { src.Currency }"));

            CreateMap<ServiceUpdateDto, Service>();
        }
    }
}
