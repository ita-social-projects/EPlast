using System;

namespace EPlast.BLL.DTO.ActiveMembership
{
    public class UserMembershipDatesDto
    {
        public string UserId { get; set; }
        public int Id { get; set; }
        public DateTime DateEntry { get; set; }
        public DateTime DateOath { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
