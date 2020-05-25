using EPlast.ViewModels.UserInformation.UserProfile;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EPlast.ViewModels.UserInformation
{
    public class EditUserViewModel
    {
        public UserViewModel User { get; set; }
        public EducationUserViewModel EducationView { get; set; }
        public WorkUserViewModel WorkView { get; set; }
        public IEnumerable<NationalityViewModel> Nationalities { get; set; }
        public IEnumerable<SelectListItem> Genders { get; set; }
        public IEnumerable<ReligionViewModel> Religions { get; set; }
        public IEnumerable<DegreeViewModel> Degrees { get; set; }
    }
}