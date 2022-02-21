using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.AnnualReport
{
    public class AnnualReportDTO
    {
        public int ID { get; set; }

        public DateTime Date { get; set; }

        public AnnualReportStatusDTO Status { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, int.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfSeatsPtashat { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, int.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfIndependentRiy { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, int.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfClubs { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, int.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfIndependentGroups { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, int.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfTeachers { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, int.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfAdministrators { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, int.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfTeacherAdministrators { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, int.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfBeneficiaries { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, int.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfPlastpryiatMembers { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, int.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfHonoraryMembers { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, double.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public double PublicFunds { get; set; } 

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, double.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public double ContributionFunds { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, double.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public double PlastSalary { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, double.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public double SponsorshipFunds { get; set; }

        [MaxLength(2000, ErrorMessage = "Максимально допустима кількість символів 2000")]
        public string ListProperty { get; set; }

        [MaxLength(2000, ErrorMessage = "Максимально допустима кількість символів 2000")]
        public string ImprovementNeeds { get; set; }

        public MembersStatisticDTO MembersStatistic { get; set; }

        public string CreatorId { get; set; }
        public UserDTO Creator { get; set; }

        public string NewCityAdminId { get; set; }
        public UserDTO NewCityAdmin { get; set; }

        [Required]
        public CityLegalStatusTypeDTO NewCityLegalStatusType { get; set; }

        public int CityId { get; set; }
        public CityDTO City { get; set; }
    }
}