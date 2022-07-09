using System;
using System.ComponentModel.DataAnnotations;
using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.BLL.DTO.ActiveMembership
{
    public class UserPlastDegreeDto
    {
        public int Id { get; set; }
        [Required]
        public PlastDegreeDto PlastDegree { get; set; }

        public DateTime DateStart { get; set; }
    }
}
