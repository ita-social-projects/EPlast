using EPlast.DataAccess.Entities;
using System;

namespace EPlast.BLL.DTO.ActiveMembership
{
    public class UserPlastDegreePostDTO
    {
        public int Id { get; set; }
        public PlastDegree PlastDegree { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateFinish { get; set; }
        public string UserId { get; set; }
    }
}
