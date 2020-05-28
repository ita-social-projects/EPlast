using EPlast.DataAccess.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.ViewModels
{
    public class ClubAdministrationViewModel
    {
        public int ID { get; set; }
        public int AdminTypeId { get; set; }
        public AdminType AdminType { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ClubId { get; set; }
        public ClubViewModel Club { get; set; }
        public ClubMembersViewModel ClubMembers { get; set; }
        public int ClubMembersID { get; set; }
    }
}
