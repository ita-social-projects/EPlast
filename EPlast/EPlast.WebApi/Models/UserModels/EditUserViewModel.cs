using EPlast.BLL.DTO.UserProfiles;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EPlast.WebApi.Models.UserModels
{
    public class EditUserViewModel
    {
        public UserViewModel User { get; set; }
        public UserEducationViewModel EducationView { get; set; }
        public UserWorkViewModel WorkView { get; set; }
        public IEnumerable<NationalityDTO> Nationalities { get; set; }
        public IEnumerable<SelectListItem> Genders { get; set; }
        public IEnumerable<ReligionDTO> Religions { get; set; }
        public IEnumerable<DegreeDTO> Degrees { get; set; }
    }
}
