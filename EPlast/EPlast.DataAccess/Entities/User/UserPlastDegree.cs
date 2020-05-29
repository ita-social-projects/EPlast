using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class UserPlastDegree
    {
        public int Id { get; set; }
        public UserPlastDegreeType UserPlastDegreeType { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime? DateFinish { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
