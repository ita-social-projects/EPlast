using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EPlast.DataAccess.Entities
{
    public class UserFormerMembershipDates
    {
        public int Id { get; set; }
        public DateTime DateEntry { get; set; }
        public DateTime DateEnd { get; set; }
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
