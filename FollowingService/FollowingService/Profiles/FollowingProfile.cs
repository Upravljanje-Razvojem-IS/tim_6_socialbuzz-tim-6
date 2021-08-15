using AutoMapper;
using FollowingService.Model.Entity;
using FollowingService.Models.DTOs;

namespace FollowingService.Profiles
{
    public class FollowingProfile :Profile
    {
        public FollowingProfile()
        {
            CreateMap<Following, FollowingDTO>();
        }
    }
}
