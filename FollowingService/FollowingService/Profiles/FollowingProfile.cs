using AutoMapper;
using FollowingService.Model.Entity;
using FollowingService.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
