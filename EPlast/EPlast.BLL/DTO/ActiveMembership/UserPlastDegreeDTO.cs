using EPlast.BLL.DTO.UserProfiles;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.ActiveMembership
{
    public class UserPlastDegreeDTO
    {
        public int Id { get; set; }
        [Required]
        public PlastDegreeDTO PlastDegree { get; set; }

        public DateTime DateStart { get; set; }
    }
}
