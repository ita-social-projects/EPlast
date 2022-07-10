using AutoMapper;
using EPlast.BLL.DTO.ActiveMembership;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.ActiveMembership
{
    public class UserPlastDegreePostProfile: Profile
    {
        public UserPlastDegreePostProfile()
        {
            CreateMap<UserPlastDegreePostDto, UserPlastDegree>().ReverseMap();
        }
    }
}
