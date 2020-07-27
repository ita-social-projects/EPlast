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
            CreateMap<RegionDTO, string>().ConvertUsing(r => r.RegionName);
            CreateMap<string, RegionDTO>().ConvertUsing(r => new RegionDTO() { RegionName = r});
        }
    }
}