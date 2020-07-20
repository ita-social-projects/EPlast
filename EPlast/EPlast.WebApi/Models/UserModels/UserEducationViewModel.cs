using EPlast.WebApi.Models.UserModels.UserProfileFields;
using System.Collections.Generic;

namespace EPlast.WebApi.Models.UserModels
{
    public class UserEducationViewModel
    {
        public int? PlaceOfStudyID { get; set; }
        public IEnumerable<EducationViewModel> PlaceOfStudyList { get; set; }
        public int? SpecialityID { get; set; }
        public IEnumerable<EducationViewModel> SpecialityList { get; set; }
    }
}
