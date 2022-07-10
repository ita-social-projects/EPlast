using AutoMapper;
using EPlast.BLL.DTO.City;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.City
{
    public class CityMembersProfile : Profile
    {
        public CityMembersProfile()
        {
            CreateMap<DatabaseEntities.CityMembers, CityMembersDto>().ReverseMap();
        }
    }
}
