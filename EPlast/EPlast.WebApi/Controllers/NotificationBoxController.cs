using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Notification;
using EPlast.BLL.Interfaces.Notifications;
using EPlast.WebApi.WebSocketHandlers;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NotificationBoxController : ControllerBase
    {
        private readonly UserNotificationHandler _userNotificationHandler;
        private readonly INotificationService _notificationService;

        public NotificationBoxController(
            INotificationService notificationService,
            UserNotificationHandler userNotificationHandler)
        {
            _notificationService = notificationService;
            _userNotificationHandler = userNotificationHandler;
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
            IEnumerable<UserNotificationDto> notificationsToAddToDb = userNotifications.Where(un => !(un.NotificationTypeId == 4 && IsOwnerOnline(un.OwnerUserId)));
            IEnumerable<UserNotificationDto> notificationsNotToAddDb = userNotifications.Except(notificationsToAddToDb);
            IEnumerable<UserNotificationDto> AddedUserNotifications;
            try
            {
                AddedUserNotifications = await _notificationService.AddListUserNotificationAsync(notificationsToAddToDb);
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
            AddedUserNotifications.Concat(notificationsNotToAddDb);
            var tasks = GetOnlineUserFromList(AddedUserNotifications).Select(un => SendPrivateNotification(un));
            await Task.WhenAll(tasks);
            return NoContent();
        }

        private IEnumerable<UserNotificationDto> GetOnlineUserFromList(IEnumerable<UserNotificationDto> userNotificationDTOs)
        {
            List<string> onlineUsers = _userNotificationHandler.GetOnlineUsers().ToList();
            return userNotificationDTOs.Where(un => onlineUsers.Contains(un.OwnerUserId));
        }

        private bool IsOwnerOnline(string ownerId)
        {
            List<string> onlineUsers = _userNotificationHandler.GetOnlineUsers().ToList();
            return onlineUsers.Contains(ownerId);
        }

        private async Task SendPrivateNotification(UserNotificationDto userNotificationDTO)
        {
           await _userNotificationHandler.SendUserNotificationAsync(userNotificationDTO);
        }
    }
}       