using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels.City;

namespace EPlast.Mapping.City
{
    public class CityMembersProfile : Profile
    {
        public CityMembersProfile()
        {
            CreateMap<CityMembersDTO, CityMembers>().ReverseMap();
            CreateMap<CityMembersViewModel, CityMembersDTO>().ReverseMap();
        }
    }
}
