using AutoMapper;
using EPlast.BLL.DTO.ActiveMembership;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.ActiveMembership
{
    public class UserPlastDegreeProfile : Profile
    {
        public UserPlastDegreeProfile()
        {
            CreateMap<UserPlastDegree, UserPlastDegreeDto>().ReverseMap();
        }
    }
}
