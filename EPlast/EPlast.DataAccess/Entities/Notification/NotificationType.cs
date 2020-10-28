using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class NotificationType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<UserNotification> UserNotifications { get; set; }
    }
}
