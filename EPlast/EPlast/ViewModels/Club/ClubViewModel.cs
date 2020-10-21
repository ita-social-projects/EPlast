using EPlast.ViewModels.Club;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.ViewModels
{
    public class ClubViewModel
    {
        public int ID { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Назва станиці не має перевищувати 50 символів")]
        public string Name { get; set; }
        [StringLength(18, ErrorMessage = "Контактний номер станиці повинен містити 12 цифр")]
        public string PhoneNumber { get; set; }
        [MaxLength(50, ErrorMessage = "Email станиці не має перевищувати 50 символів")]
        public string Email { get; set; }
        [MaxLength(256, ErrorMessage = "Посилання на web-сторінку станиці не має перевищувати 256 символів")]
        public string ClubURL { get; set; }
        [MaxLength(1024, ErrorMessage = "Історія станиці не має перевищувати 1024 символів")]
        public string Description { get; set; }
        [Required, MaxLength(60, ErrorMessage = "Назва вулиці розташування станиці не має перевищувати 60 символів")]
        public string Street { get; set; }
        [Required, MaxLength(10, ErrorMessage = "Номер будинку розташування станиці не має перевищувати 10 символів")]
        public string HouseNumber { get; set; }
        [MaxLength(10, ErrorMessage = "Номер офісу/квартири розташування станиці не має перевищувати 10 символів")]
        public string OfficeNumber { get; set; }
        [MaxLength(7, ErrorMessage = "Поштовий індекс станиці не має перевищувати 7 символів")]
        public string PostIndex { get; set; }
        [StringLength(int.MaxValue, MinimumLength = 3)]
        public string Logo { get; set; }
        public ICollection<ClubDocumentViewModel> ClubDocuments { get; set; }
        public ICollection<ClubMembersViewModel> ClubMembers { get; set; }
        public ICollection<ClubAdministrationViewModel> ClubAdministration { get; set; }
        public ICollection<ClubLegalStatusViewModel> ClubLegalStatuses { get; set; }
    }
}