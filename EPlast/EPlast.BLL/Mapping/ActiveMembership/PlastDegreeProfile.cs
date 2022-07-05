using AutoMapper;
using EPlast.BLL.DTO.ActiveMembership;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.ActiveMembership
{
    public class PlastDegreeProfile : Profile
    {
        public PlastDegreeProfile()
        {
            CreateMap<PlastDegreeDto, PlastDegree>().ReverseMap();
        }
    }
}
