using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPlast.DataAccess.Entities
{
    public class AnnualReport
    {
        public int ID { get; set; }

        public DateTime Date { get; set; }

        public AnnualReportStatus Status { get; set; }

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
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PublicFunds { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ContributionFunds { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PlastSalary { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SponsorshipFunds { get; set; }

        [MaxLength(2000, ErrorMessage = "Максимально допустима кількість символів 2000")]
        public string ListProperty { get; set; }

        [MaxLength(2000, ErrorMessage = "Максимально допустима кількість символів 2000")]
        public string ImprovementNeeds { get; set; }

        public MembersStatistic MembersStatistic { get; set; }

        public string CreatorId { get; set; }
        public User Creator { get; set; }
        public string CreatorFirstName { get; set; }
        public string CreatorLastName { get; set; }
        public string CreatorFatherName { get; set; }

        public string NewCityAdminId { get; set; }
        public User NewCityAdmin { get; set; }

        [Required]
        public CityLegalStatusType NewCityLegalStatusType { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }
    }
}
