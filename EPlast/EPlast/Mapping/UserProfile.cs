using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.Mapping
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            //CreateMap<User, UserDTO>().ForMember(e => e.Id, f => f.MapFrom(a => a.Id));
            //CreateMap<UserDTO, User>().ForMember(e => e.Id, f => f.Ignore());
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<UserViewModel, UserDTO>();
            CreateMap<UserDTO, UserViewModel>();
        }
    }
}
