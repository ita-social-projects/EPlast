using EPlast.WebApi.Models.Admin;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.Club
{
    public class ClubAdministrationViewModel
    {
        public int ID { get; set; }
        public int AdminTypeId { get; set; }
        public AdminTypeViewModel AdminType { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ClubId { get; set; }
        public ClubMembersViewModel ClubMembers { get; set; }
        public int ClubMembersID { get; set; }
    }
}
