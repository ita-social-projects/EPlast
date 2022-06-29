using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EPlast.Resources;

namespace EPlast.DataAccess.Entities
{
    public class City
    {
        public int ID { get; set; }

        [Required, MaxLength(50, ErrorMessage = "Назва станиці не має перевищувати 50 символів")]
        public string Name { get; set; }

        [StringLength(18, ErrorMessage = "Контактний номер станиці повинен містити 12 цифр")]
        public string PhoneNumber { get; set; }

        [MaxLength(50, ErrorMessage = "Email станиці не має перевищувати 50 символів")]
        public string Email { get; set; }

        [MaxLength(256, ErrorMessage = "Посилання на web-сторінку станиці не має перевищувати 256 символів")]
        public string CityURL { get; set; }

        [MaxLength(1024, ErrorMessage = "Історія станиці не має перевищувати 1024 символів")]
        public string Description { get; set; }

        [Required, MaxLength(60, ErrorMessage = "Назва адреси розташування станиці не має перевищувати 60 символів")]
        public string Address { get; set; }

        public UkraineOblasts Oblast { get; set; }

        public CityLevel Level { get; set; }

        [StringLength(int.MaxValue, MinimumLength = 3)]
        public string Logo { get; set; }
        public bool IsActive { get; set; }

        public ICollection<CityDocuments> CityDocuments { get; set; }
        public ICollection<CityMembers> CityMembers { get; set; }
        public ICollection<CityAdministration> CityAdministration { get; set; }
        public ICollection<AnnualReport> AnnualReports { get; set; }
        public ICollection<CityLegalStatus> CityLegalStatuses { get; set; }
        public ICollection<UserRenewal> UserRenewals { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }
    }
}
