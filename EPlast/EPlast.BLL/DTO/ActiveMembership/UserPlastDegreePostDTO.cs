using System;

namespace EPlast.BLL.DTO.ActiveMembership
{
    public class UserPlastDegreePostDto
    {
        public int Id { get; set; }
        public int PlastDegreeId { get; set; }
        public DateTime DateStart { get; set; }
        public string UserId { get; set; }
    }
}
