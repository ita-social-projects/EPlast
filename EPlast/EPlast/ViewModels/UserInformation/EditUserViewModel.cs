using EPlast.DataAccess.Entities;
using EPlast.ViewModels.UserInformation.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels.UserInformation
{
    public class EditUserViewModel
    {
        public User User { get; set; }
        public EducationUserViewModel EducationView { get; set; }
        public WorkUserViewModel WorkView { get; set; }
        public IEnumerable<NationalityViewModel> Nationalities { get; set; }
        public IEnumerable<GenderViewModel> Genders { get; set; }
        public IEnumerable<ReligionViewModel> Religions { get; set; }
        public IEnumerable<DegreeViewModel> Degrees { get; set; }
    }
}