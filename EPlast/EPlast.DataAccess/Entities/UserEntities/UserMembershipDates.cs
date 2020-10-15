using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Entities
{
    public class UserMembershipDates
    {
        public int Id { get; set; }
        public DateTime DateEntry { get; set; }
        public DateTime DateOath { get; set; }
        public DateTime DateEnd { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
