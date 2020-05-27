using System.Collections.Generic;

namespace EPlast.ViewModels.City
{
    public class CityProfileViewModel
    {
        public CityViewModel City { get; set; }
        public CityAdministrationViewModel CityHead { get; set; }
        public List<CityAdministrationViewModel> CityAdmins { get; set; }
        public List<CityMembersViewModel> Members { get; set; }
        public List<CityMembersViewModel> Followers { get; set; }
        public List<CityDocumentViewModel> CityDoc { get; set; }

    }
}
