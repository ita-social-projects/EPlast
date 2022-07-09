using AutoMapper;
using EPlast.BLL.DTO.Notification;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Notification
{
    public class NotificationTypeProfile : Profile
    {
        public NotificationTypeProfile()
        {
            CreateMap<NotificationType, NotificationTypeDto>().ReverseMap();
        }
    }
}
