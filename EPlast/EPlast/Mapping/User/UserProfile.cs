using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.Mapping
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<DataAccess.Entities.User, UserDTO>();
            CreateMap<UserDTO, DataAccess.Entities.User>();
            CreateMap<UserViewModel, UserDTO>();
            CreateMap<UserDTO, UserViewModel>();
        }
    }
}
