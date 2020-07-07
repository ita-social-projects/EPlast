using EPlast.WebApi.Models.Region;
using System.Collections.Generic;

namespace EPlast.WebApi.Models.City
{
    public class CityViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CityURL { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string OfficeNumber { get; set; }
        public string PostIndex { get; set; }
        public string Logo { get; set; }
        public int RegionId { get; set; }
        public RegionViewModel Region { get; set; }
        public CityAdministrationViewModel Head { get; set; }
        public IEnumerable<CityAdministrationViewModel> Administration { get; set; }
        public IEnumerable<CityMembersViewModel> Members { get; set; }
        public IEnumerable<CityMembersViewModel> Followers { get; set; }
        public IEnumerable<CityDocumentsViewModel> Documents { get; set; }

        public void SetMembersAndAdministration(CityProfileViewModel cityProfile)
        {
            Head = cityProfile.CityHead;
            Administration = cityProfile.CityAdmins;
            Members = cityProfile.Members;
            Followers = cityProfile.Followers;
            Documents = cityProfile.CityDoc;
        }
    }
}
