using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Notification;
using EPlast.BLL.Interfaces.Notifications;
using EPlast.WebApi.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace EPlast.WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NotificationBoxController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;

        public NotificationBoxController(
            INotificationService notificationService,
            IHubContext<NotificationHub, INotificationHub> hubContext)
        {
            _notificationService = notificationService;
            _hubContext = hubContext;
        }

        [HttpGet("getTypes")]
        public async Task<IActionResult> GetAllTypes()
        {
            return Ok(await _notificationService.GetAllNotificationTypesAsync());
        }

        [HttpGet("getNotifications/{userId}")]
        public async Task<IActionResult> GetAllUserNotification(string userId)
        {
            return Ok(await _notificationService.GetAllUserNotificationsAsync(userId));
        }

        [HttpDelete("removeNotification/{notificationId}")]
        public async Task<IActionResult> RemoveUserNotification(int notificationId)
        {
            if (await _notificationService.RemoveUserNotificationAsync(notificationId))
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete("removeAllNotifications/{userId}")]
        public async Task<IActionResult> RemoveAllUserNotifications(string userId)
        {
            if (await _notificationService.RemoveAllUserNotificationAsync(userId))
            {
                return NoContent();
            }

            return BadRequest();
        }
        [HttpPost("setCheckNotifications/setChecked/{userId}")]
        public async Task<IActionResult> SetCheckForListNotification(string userId)
        {
            if (await _notificationService.SetCheckForListNotificationAsync(userId))
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPost("addNotifications")]
        public async Task<IActionResult> AddNotificationList(IEnumerable<UserNotificationDto> userNotifications)
        {
            IEnumerable<UserNotificationDto> addedUserNotifications;
            try
            {
                addedUserNotifications = await _notificationService.AddListUserNotificationAsync(userNotifications);
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }

            foreach (UserNotificationDto notification in addedUserNotifications)
            {
                await _hubContext.Clients.User(notification.OwnerUserId).AddNotification(notification);
            }
            return NoContent();
        }
    }
}