using System.Collections.Generic;

namespace EPlast.WebApi.Models.City
{
    public class CityProfileViewModel
    {
        public CityViewModel City { get; set; }
        public CityAdministrationViewModel CityHead { get; set; }
        public List<CityAdministrationViewModel> CityAdmins { get; set; }
        public List<CityMembersViewModel> Members { get; set; }
        public List<CityMembersViewModel> Followers { get; set; }
        public List<CityDocumentsViewModel> CityDoc { get; set; }

        public void SetMembersAndAdministration()
        {
            City.SetMembersAndAdministration(this);
        }
    }
}
