using AutoMapper;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.ViewModels.EventUser;

namespace EPlast.Mapping.EventUser
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, UserViewModel>().ReverseMap();
        }
    }
}
