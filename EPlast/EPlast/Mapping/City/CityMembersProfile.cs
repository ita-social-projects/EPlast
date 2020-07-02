using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.ViewModels.City;

namespace EPlast.Mapping.City
{
    public class CityMembersProfile : Profile
    {
        public CityMembersProfile()
        {
            CreateMap<CityMembersViewModel, CityMembersDTO>().ReverseMap();
        }
    }
}
