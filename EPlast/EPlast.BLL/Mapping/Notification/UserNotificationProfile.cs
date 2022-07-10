using AutoMapper;
using EPlast.BLL.DTO.Notification;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Notification
{
    public class UserNotificationProfile : Profile
    {
        public UserNotificationProfile()
        {
            CreateMap<UserNotification, UserNotificationDto>().ReverseMap();
        }
    }

}
