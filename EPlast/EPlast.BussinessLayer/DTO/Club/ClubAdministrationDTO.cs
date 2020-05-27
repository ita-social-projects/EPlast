using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BussinessLayer.DTO.Club
{
    public class CLubAdministrationDTO
    {
        public int ID { get; set; }
        public int AdminTypeId { get; set; }
        public AdminType AdminType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ClubId { get; set; }
        public ClubDTO Club { get; set; }
        public ClubMembersDTO ClubMembers { get; set; }
        public int ClubMembersID { get; set; }
    }
}
