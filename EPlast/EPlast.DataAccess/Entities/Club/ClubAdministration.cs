using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class ClubAdministration
    {
        public int ID { get; set; }
        public int AdminTypeId { get; set; }
        public AdminType AdminType { get; set; }
        [Required] public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ClubId { get; set; }
        public Club Club { get; set; }
        public ClubMembers ClubMembers { get; set; }
        public int ClubMembersID { get; set; }
    }
}