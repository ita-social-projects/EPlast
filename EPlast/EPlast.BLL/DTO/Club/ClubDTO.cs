using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.Club
{
    public class ClubDTO
    {
        public int ID { get; set; }

        [Required, MaxLength(50, ErrorMessage = "Назва куреня не має перевищувати 50 символів")]
        public string Name { get; set; }

        [StringLength(18, ErrorMessage = "Контактний номер куреня повинен містити 12 цифр")]
        public string PhoneNumber { get; set; }

        [MaxLength(50, ErrorMessage = "Email куреня не має перевищувати 50 символів")]
        public string Email { get; set; }

        [MaxLength(256, ErrorMessage = "Посилання на web-сторінку куреня не має перевищувати 256 символів")]
        public string ClubURL { get; set; }

        [MaxLength(1024, ErrorMessage = "Історія куреня не має перевищувати 1024 символів")]
        public string Description { get; set; }

        [Required, MaxLength(60, ErrorMessage = "Назва вулиці розташування куреня не має перевищувати 60 символів")]
        public string Street { get; set; }

        [Required, MaxLength(10, ErrorMessage = "Номер будинку розташування куреня не має перевищувати 10 символів")]
        public string HouseNumber { get; set; }

        [MaxLength(10, ErrorMessage = "Номер офісу/квартири розташування куреня не має перевищувати 10 символів")]
        public string OfficeNumber { get; set; }

        [MaxLength(7, ErrorMessage = "Поштовий індекс куреня не має перевищувати 7 символів")]
        public string PostIndex { get; set; }
        public string Logo { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanJoin { get; set; }
        public int MemberCount { get; set; }
        public int FollowerCount { get; set; }
        public int AdministrationCount { get; set; }
        public int DocumentsCount { get; set; }
        public IEnumerable<ClubDocumentsDTO> ClubDocuments { get; set; }
        public IEnumerable<ClubMembersDTO> ClubMembers { get; set; }
        public IEnumerable<ClubAdministrationDTO> ClubAdministration { get; set; }
        public IEnumerable<ClubLegalStatusDTO> ClubLegalStatuses { get; set; }
    }
}