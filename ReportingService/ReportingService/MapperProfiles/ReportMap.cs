using AutoMapper;
using ReportingService.DTOs;
using ReportingService.Entities;

namespace ReportingService.MapperProfiles
{
    public class ReportMap : Profile
    {
        public ReportMap()
        {
            CreateMap<Report, ReportGet>();
            CreateMap<Report, ReportConfirm>();

        }
    }
}
