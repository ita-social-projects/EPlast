using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels
{
    public class EducationViewModel
    {
        public int? PlaceOfStudyID { get; set; }
        public IEnumerable<Education> PlaceOfStudyList { get; set; }
        public int? SpecialityID { get; set; }
        public IEnumerable<Education> SpecialityList { get; set; }
    }
}
