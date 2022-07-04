using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Notification;

namespace EPlast.BLL.Interfaces.Notifications
{
    /// <summary>
    /// Implement  operations for work with notifications
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Returns all notification types
        /// </summary>
        /// <returns>All notification types</returns>
        public Task<IEnumerable<NotificationTypeDto>> GetAllNotificationTypesAsync();
        /// <summary>
        /// Returns all notifications of current user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>all notifications of current user</returns>
        public Task<IEnumerable<UserNotificationDto>> GetAllUserNotificationsAsync(string userId);
        /// <summary>
        /// Returns list of added notifications
        /// </summary>
        /// <param name="userNotificationsDTO">List of userNotificationDTO</param>
        /// <returns>Returns list of UserNotificationDTO</returns>
        public Task<IEnumerable<UserNotificationDto>> AddListUserNotificationAsync(IEnumerable<UserNotificationDto> userNotificationsDTO);
        /// <summary>
        /// Returns bool, if check for list of notification set successfull, return true, else false
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Returns bool</returns>
        public Task<bool> SetCheckForListNotificationAsync(string userId);
        /// <summary>
        /// Returns bool, if user notification removed successfull, return true, else false
        /// </summary>
        /// <param name="notificationId">Notification id</param>
        /// <returns>Returns bool</returns>
        public Task<bool> RemoveUserNotificationAsync(int notificationId);
        /// <summary>
        /// Returns bool, if list of user notifications removed successfull, return true, else false
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Returns bool</returns>
        public Task<bool> RemoveAllUserNotificationAsync(string userId);
    }
}
