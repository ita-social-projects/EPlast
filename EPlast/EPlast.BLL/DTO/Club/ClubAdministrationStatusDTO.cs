using System;
using EPlast.BLL.DTO.Admin;

namespace EPlast.BLL.DTO.Club
{
    public class ClubAdministrationStatusDto
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public ClubUserDto User { get; set; }
        public int ClubId { get; set; }
        public ClubDto Club { get; set; }
        public int AdminTypeId { get; set; }
        public AdminTypeDto AdminType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Status { get; set; }
    }
}
