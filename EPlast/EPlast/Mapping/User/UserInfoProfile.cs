using AutoMapper;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.BussinessLayer.DTO.UserProfiles;
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