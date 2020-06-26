using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.WebApi.Models.User;

namespace EPlast.WebApi.Mapping.User
{
    public class UserInfoProfile : Profile
    {
        public UserInfoProfile()
        {
            CreateMap<EPlast.DataAccess.Entities.User, UserDTO>().ReverseMap();
            CreateMap<UserDTO, UserInfoViewModel>().ReverseMap();
        }
    }
}