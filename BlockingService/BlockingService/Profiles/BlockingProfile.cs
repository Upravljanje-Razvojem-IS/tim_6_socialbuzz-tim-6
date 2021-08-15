using AutoMapper;
using BlockingService.Model.Entity;
using BlockingService.Models.DTOs;

namespace BlockingService.Profiles
{
    public class BlockingProfile :Profile
    {
        public BlockingProfile()
        {
            CreateMap<Blocking, BlockingDTO>();
        }
    }
}
