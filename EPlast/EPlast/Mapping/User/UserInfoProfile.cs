using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.ViewModels.UserProfile;

namespace EPlast.Mapping
{
    public class UserInfoProfile : Profile
    {
        public UserInfoProfile()
        {
            CreateMap<UserInfoViewModel, UserDTO>().ReverseMap();
        }
    }
}