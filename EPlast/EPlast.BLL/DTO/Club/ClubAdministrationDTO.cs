using System;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.Interfaces.Club;

namespace EPlast.BLL.DTO.Club
{
    public class ClubAdministrationDto : IClubMember
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public ClubUserDto User { get; set; }
        public int ClubId { get; set; }
        public int AdminTypeId { get; set; }
        public ClubDto Club { get; set; }
        public AdminTypeDto AdminType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Status { get; set; }
        
    }
}