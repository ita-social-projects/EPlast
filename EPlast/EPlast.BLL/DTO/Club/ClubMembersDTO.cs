using System;
using EPlast.BLL.Interfaces.Club;

namespace EPlast.BLL.DTO.Club
{
    public class ClubMembersDto : IClubMember
    {
        public int ID { get; set; }
        public bool IsApproved { get; set; }
        public string UserId { get; set; }
        public ClubUserDto User { get; set; }
        public string ClubId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}