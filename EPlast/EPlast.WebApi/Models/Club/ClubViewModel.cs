using System.Collections.Generic;

namespace EPlast.WebApi.Models.Club
{
    public class ClubViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ClubURL { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string OfficeNumber { get; set; }
        public string PostIndex { get; set; }
        public string Logo { get; set; }
        public string Region { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
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