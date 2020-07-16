using EPlast.BLL.DTO.UserProfiles;
using EPlast.WebApi.Models.UserModels.UserProfileFields;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EPlast.WebApi.Models.UserModels
{
    public class EditUserViewModel
    {
        public UserViewModel User { get; set; }
        public UserEducationViewModel EducationView { get; set; }
        public UserWorkViewModel WorkView { get; set; }
        public IEnumerable<NationalityViewModel> Nationalities { get; set; }
        public IEnumerable<GenderViewModel> Genders { get; set; }
        public IEnumerable<ReligionViewModel> Religions { get; set; }
        public IEnumerable<DegreeViewModel> Degrees { get; set; }
        public string ImageBase64 { get; set; }
    }
}
