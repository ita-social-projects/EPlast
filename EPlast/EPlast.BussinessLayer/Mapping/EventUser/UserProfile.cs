using AutoMapper;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping.EventUser
{
    class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
        }
    }
}
