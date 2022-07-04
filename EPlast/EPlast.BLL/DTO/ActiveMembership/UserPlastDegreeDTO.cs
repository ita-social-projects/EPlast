using System;
using System.ComponentModel.DataAnnotations;

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
