using AutoMapper;
using PostService.Entities.Posts;
using PostService.Models.DTOs.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Profiles
{
    public class ProductProfile: Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, PostDto>().ForMember(
                dest => dest.PostType,
                opt => opt.MapFrom(src => "Proizvod"));

            CreateMap<Product, Post>();
            CreateMap<ProductCreationDto, Product>();
            CreateMap<Product, ProductConfirmationDto>().ForMember(
                    dest => dest.Price,
                    opt => opt.MapFrom(src => $"{ src.Price } { src.Currency }"));

            CreateMap<ProductUpdateDto, Product>();
        }
    }
}
