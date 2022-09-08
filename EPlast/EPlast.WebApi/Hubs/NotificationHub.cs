using EPlast.BLL.DTO.Notification;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.WebApi.Hubs
{
    public class NotificationHub : Hub<INotificationHub>
    {
    }

    public interface INotificationHub
    {
        public Task AddNotification(UserNotificationDto userNotifications);
    }
}