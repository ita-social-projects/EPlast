using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EPlast.BLL.DTO.Notification
{
    public class UserNotificationDTO
    {
        public int Id { get; set; }
        [Required]
        public string OwnerUserId { get; set; }
        [Required]
        public NotificationTypeDTO NotificationType { get; set; }
        public bool Checked { get; set; }
        public string Message { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime? CheckedAt { get; set; }
        public string SenderUserId { get; set; }
    }
}
