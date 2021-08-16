using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Club
    {
        public int ID { get; set; }
        [Required, MaxLength(200, ErrorMessage = "Назва куреня не має перевищувати 200 символів")]
        public string Name { get; set; }
        [StringLength(18, ErrorMessage = "Контактний номер куреня повинен містити 12 цифр")]
        public string PhoneNumber { get; set; }
        [MaxLength(50, ErrorMessage = "Email куреня не має перевищувати 50 символів")]
        public string Email { get; set; }
        [MaxLength(256, ErrorMessage = "Посилання на web-сторінку куреня не має перевищувати 256 символів")]
        public string ClubURL { get; set; }
        [MaxLength(1024, ErrorMessage = "Історія куреня не має перевищувати 1024 символів")]
        public string Description { get; set; }
        [MaxLength(500, ErrorMessage = "Гасло куреня не має перевищувати 500 символів")]
        public string Slogan { get; set; }
        [StringLength(int.MaxValue, MinimumLength = 3)]
        public string Logo { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ClubDocuments> ClubDocuments { get; set; }
        public ICollection<ClubMembers> ClubMembers { get; set; }
        public ICollection<ClubAdministration> ClubAdministration { get; set; }
        public ICollection<AnnualReport> AnnualReports { get; set; }
        public ICollection<ClubLegalStatus> ClubLegalStatuses { get; set; }
    }
}
