using AutoMapper;
using EPlast.BLL.DTO.ActiveMembership;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.ActiveMembership
{
    class UserMembershipDatesProfile : Profile
    {
        public UserMembershipDatesProfile()
        {
            CreateMap<UserMembershipDatesDto, UserMembershipDates>().ReverseMap();
        }
    }
}
