using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.AnnualReport;
using EPlast.ViewModels.AnnualReport;
using EPlast.DataAccess.Entities;

namespace EPlast.Mapping.AnnualReport
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<RegionViewModel, RegionDTO>().ReverseMap();
        }
    }
}