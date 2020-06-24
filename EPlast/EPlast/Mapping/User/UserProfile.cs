using AutoMapper;
using EPlast.BusinessLogicLayer.DTO;
using EPlast.BusinessLogicLayer.DTO.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.Mapping
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<UserViewModel, UserDTO>();
            CreateMap<UserDTO, UserViewModel>();
        }
    }
}
