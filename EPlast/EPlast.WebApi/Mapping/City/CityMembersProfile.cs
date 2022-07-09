using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.WebApi.Models.City;

namespace EPlast.BLL.Mapping.City
{
    public class CityMembersProfile : Profile
    {
        public CityMembersProfile()
        {
            CreateMap<CityMembersViewModel, CityMembersDto>().ReverseMap();
        }
    }
}
