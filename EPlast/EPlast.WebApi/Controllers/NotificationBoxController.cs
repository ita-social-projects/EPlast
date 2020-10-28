using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.Notifications;
using System.Collections.Generic;
using EPlast.BLL.DTO.Notification;
using Microsoft.AspNetCore.SignalR;
using EPlast.WebApi.SignalRHubs;
using System.Linq;
using EPlast.WebApi.WebSocketHandlers;

namespace EPlast.WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NotificationBoxController : ControllerBase
    {
        private UserNotificationHandler _userNotificationHandler { get; set; }
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
        [HttpPost("setCheckNotifications/setChecked")]
        public async Task<IActionResult> SetCheckForListNotification(IEnumerable<int> notificationIdList)
        {
            if (await _notificationService.SetCheckForListNotificationAsync(notificationIdList))
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPost("addNotifications")]
        public async Task<IActionResult> AddNotificationList(IEnumerable<UserNotificationDTO> userNotifications)
        {
            IEnumerable<UserNotificationDTO> AddedUserNotifications;
            try
            {
                AddedUserNotifications = await _notificationService.AddListUserNotificationAsync(userNotifications);
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }

            var tasks = GetOnlineUserFromList(AddedUserNotifications).Select(un => SendPrivateNotification(un));
            await Task.WhenAll(tasks);
            return NoContent();
        }

        private IEnumerable<UserNotificationDTO> GetOnlineUserFromList(IEnumerable<UserNotificationDTO> userNotificationDTOs)
        {
            List<string> onlineUsers = _userNotificationHandler.GetOnlineUsers().ToList();
            return userNotificationDTOs.Where(un => onlineUsers.Contains(un.OwnerUserId));
        }


        private async Task SendPrivateNotification(UserNotificationDTO userNotificationDTO)
        {
           await _userNotificationHandler.SendUserNotificationAsync(userNotificationDTO);
        }
    }
}       