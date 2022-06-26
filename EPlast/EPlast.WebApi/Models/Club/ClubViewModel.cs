using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.Club
{
    public class ClubViewModel
    {
        public int ID { get; set; }
        [Required, MaxLength(200)]
        public string Name { get; set; }
        [Phone, StringLength(18)]
        public string PhoneNumber { get; set; }
        [Required, EmailAddress, MaxLength(50)]
        public string Email { get; set; }
        [Url, MaxLength(256)]
        public string ClubURL { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public string Slogan { get; set; }
        public string Logo { get; set; }
        public bool isActive { get; set; }
        public string Region { get; set; }
        public bool CanJoin { get; set; }
        public int MemberCount { get; set; }
        public int FollowerCount { get; set; }
        public int AdministrationCount { get; set; }
        public int DocumentsCount { get; set; }
        public ClubAdministrationViewModel Head { get; set; }
        public ClubAdministrationViewModel HeadDeputy { get; set; }
        public IEnumerable<ClubAdministrationViewModel> Administration { get; set; }
        public IEnumerable<ClubMembersViewModel> Members { get; set; }
        public IEnumerable<ClubMembersViewModel> Followers { get; set; }
        public IEnumerable<ClubDocumentsViewModel> Documents { get; set; }
    }
}