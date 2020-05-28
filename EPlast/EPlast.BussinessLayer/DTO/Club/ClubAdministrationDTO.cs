using System;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.DTO
{
    public class ClubAdministrationDTO
    {
        /*було
        public int adminId { get; set; }

        public int clubIndex { get; set; }

        public DateTime? enddate { get; set; }
        public DateTime startdate { get; set; }
        public string AdminType { get; set; }
        */

        public int ID { get; set; }
        public int AdminTypeId { get; set; }
        public string AdminTypeName { get; set; }
        public AdminType AdminType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ClubId { get; set; }
        public ClubDTO Club { get; set; }
        public ClubMembersDTO ClubMembers { get; set; }
        public int ClubMembersID { get; set; }
    }
}