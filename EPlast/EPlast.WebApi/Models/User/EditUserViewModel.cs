using EPlast.BussinessLayer.DTO.UserProfiles;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EPlast.WebApi.Models.User
{
    public class EditUserViewModel
    {
        public UserDTO User { get; set; }
        public UserEducationViewModel EducationView { get; set; }
        public UserWorkViewModel WorkView { get; set; }
        public IEnumerable<NationalityDTO> Nationalities { get; set; }
        public IEnumerable<SelectListItem> Genders { get; set; }
        public IEnumerable<ReligionDTO> Religions { get; set; }
        public IEnumerable<DegreeDTO> Degrees { get; set; }
    }
}
