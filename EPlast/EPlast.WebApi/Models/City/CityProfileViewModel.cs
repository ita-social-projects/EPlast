using System.Collections.Generic;

namespace EPlast.WebApi.Models.City
{
    public class CityProfileViewModel
    {
        public CityViewModel City { get; set; }
        public CityAdministrationViewModel Head { get; set; }
        public List<CityAdministrationViewModel> Admins { get; set; }
        public List<CityMembersViewModel> Members { get; set; }
        public List<CityMembersViewModel> Followers { get; set; }
        public List<CityDocumentsViewModel> Documents { get; set; }
    }
}
