using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.UserProfiles
{
    public class UserProfileDTO
    {
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата народження")]
        [Required(ErrorMessage = "Поле дата народження є обов'язковим")]
        public DateTime? Birthday { get; set; }

        public int? EducationId { get; set; }
        public EducationDTO Education { get; set; }
        public int? DegreeId { get; set; }
        public DegreeDTO Degree { get; set; }
        public int? NationalityId { get; set; }
        public NationalityDTO Nationality { get; set; }
        public int? ReligionId { get; set; }
        public ReligionDTO Religion { get; set; }
        public int? WorkId { get; set; }
        public WorkDTO Work { get; set; }
        public int? GenderID { get; set; }
        public GenderDTO Gender { get; set; }
        public int? UpuDegreeID { get; set; }
        public UpuDegreeDTO UpuDegree { get; set; }

        [Display(Name = "Домашня адреса")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`0-9.-]{1,31}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`0-9.-]{1,31})*$",
            ErrorMessage = "Домашня адреса має містити тільки літери та цифри")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Домашня адреса повинна складати від 3 до 30 символів")]
        [Required(ErrorMessage = "Поле домашня адреса є обов'язковим")]
        public string Address { get; set; }

        public string UserID { get; set; }

        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,31})*$",
            ErrorMessage = "Псевдо має містити тільки літери")]
        [MaxLength(30, ErrorMessage = "Псевдо не може перевищувати 30 символів")]
        public string Pseudo { get; set; }

        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`0-9.-]{1,51}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`0-9.-]{1,51})*$",
            ErrorMessage = "Поле Громадська, політична діяльність має містити тільки літери та цифри")]
        [MaxLength(50, ErrorMessage = "Поле Громадська, політична діяльність не може перевищувати 50 символів")]
        public string PublicPoliticalActivity { get; set; }

        public string FacebookLink { get; set; }
        public string TwitterLink { get; set; }
        public string InstagramLink { get; set; }
        public UserDTO User { get; set; }
        public AreaDTO Area { get; set; }
    }
}
