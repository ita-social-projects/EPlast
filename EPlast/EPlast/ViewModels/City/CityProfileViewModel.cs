using System.Collections.Generic;

namespace EPlast.ViewModels.City
{
    public class CityProfileViewModel
    {
        public CityViewModel City { get; set; }
        public CityAdministrationViewModel Head { get; set; }
        public List<CityAdministrationViewModel> Admins { get; set; }
        public List<CityMembersViewModel> Members { get; set; }
        public List<CityMembersViewModel> Followers { get; set; }
        public List<CityDocumentViewModel> Documents { get; set; }
    }
}
