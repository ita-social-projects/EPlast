using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;

namespace EPlast.WebApi.Models.User
{
    public class UserEducationViewModel
    {
        public int? PlaceOfStudyID { get; set; }
        public IEnumerable<EducationDTO> PlaceOfStudyList { get; set; }
        public int? SpecialityID { get; set; }
        public IEnumerable<EducationDTO> SpecialityList { get; set; }
    }
}
