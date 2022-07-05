using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.WebApi.Models.Region;

namespace EPlast.BLL.Mapping.Region
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<RegionViewModel, RegionDto>().ReverseMap();
            CreateMap<RegionDto, string>().ConvertUsing(r => r.RegionName);
            CreateMap<string, RegionDto>().ConvertUsing(r => new RegionDto() { RegionName = r });
        }
    }
}