using AutoMapper;
using EPlast.BLL.DTO.EventUser;
using EPlast.ViewModels.EventUser;

namespace EPlast.Mapping.EventUser
{
    public class UserInfoProfile : Profile
    {
        public UserInfoProfile()
        {
            CreateMap<DataAccess.Entities.User, UserInfoDTO>().ReverseMap();
            CreateMap<UserInfoViewModel, UserInfoDTO>().ReverseMap();
        }

    }
}
