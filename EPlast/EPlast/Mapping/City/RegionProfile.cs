using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.DataAccess.Entities;

namespace EPlast.Mapping
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<Region, RegionDTO>().ReverseMap();
        }
    }
}