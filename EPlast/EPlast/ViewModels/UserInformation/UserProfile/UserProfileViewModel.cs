using System;
using System.ComponentModel.DataAnnotations;


namespace EPlast.ViewModels.UserInformation.UserProfile
{
    public class UserProfileViewModel
    {
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата народження")]
        public DateTime? Birthday { get; set; }
        public int? EducationId { get; set; }
        public EducationViewModel Education { get; set; }
        public int? DegreeId { get; set; }
        public DegreeViewModel Degree { get; set; }
        public int? NationalityId { get; set; }
        public NationalityViewModel Nationality { get; set; }
        public int? ReligionId { get; set; }
        public ReligionViewModel Religion { get; set; }
        public int? WorkId { get; set; }
        public WorkViewModel Work { get; set; }
        public int? GenderID { get; set; }
        public GenderViewModel Gender { get; set; }
        [Display(Name = "Домашня адреса")]
        [MaxLength(50, ErrorMessage = "Адреса не може перевищувати 50 символів")]
        public string Address { get; set; }
        public string UserID { get; set; }
        public UserViewModel User { get; set; }
    }
}
