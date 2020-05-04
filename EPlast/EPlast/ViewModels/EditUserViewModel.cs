using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels
{
    public class EditUserViewModel
    {
        public User User { get; set; }
        public EducationViewModel EducationView { get; set; }
        public WorkViewModel WorkView { get; set; }
        public IEnumerable<Nationality> Nationalities { get; set; }
        public IEnumerable<Gender> Genders { get; set; }
        public IEnumerable<Religion> Religions { get; set; }
        public IEnumerable<Degree> Degrees { get; set; }
        public string Birthday { get; set; }
    }
}