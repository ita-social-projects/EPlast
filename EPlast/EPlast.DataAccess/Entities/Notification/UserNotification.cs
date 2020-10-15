using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class UserNotification
    {
        public int Id { get; set; }
        [Required]
        public string OwnerUserId { get; set; }
        [Required]
        public NotificationType NotificationType { get; set; }
        public bool Checked { get; set; }
        public string Message { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime? CheckedAt { get; set; }
        public string SenderUserId { get; set; }

    }
}
