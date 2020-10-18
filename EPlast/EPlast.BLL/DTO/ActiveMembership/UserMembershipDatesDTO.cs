using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.ActiveMembership
{
    public class UserMembershipDatesDTO
    {
        public int Id { get; set; }
        public DateTime DateEntry { get; set; }
        public DateTime DateOath { get; set; }
        public DateTime DateEnd { get; set; }
        public string UserId { get; set; }
    }
}
