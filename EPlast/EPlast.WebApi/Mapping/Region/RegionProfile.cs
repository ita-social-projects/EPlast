using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.WebApi.Models.Region;

namespace EPlast.BLL.Mapping.Region
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<RegionViewModel, RegionDTO>().ReverseMap();
        }
    }
}