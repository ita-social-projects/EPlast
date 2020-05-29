using EPlast.ViewModels.UserInformation.UserProfile;
using System.Collections.Generic;

namespace EPlast.ViewModels.UserInformation
{
    public class EducationUserViewModel
    {
        public int? PlaceOfStudyID { get; set; }
        public IEnumerable<EducationViewModel> PlaceOfStudyList { get; set; }
        public int? SpecialityID { get; set; }
        public IEnumerable<EducationViewModel> SpecialityList { get; set; }
    }
}
