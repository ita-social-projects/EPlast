using EPlast.BLL.DTO.Region;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EPlast.BLL.DTO.UserProfiles;
using Newtonsoft.Json;

namespace EPlast.BLL.DTO.City
{
    public class CityDTO
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

        [Required, MaxLength(60, ErrorMessage = "Назва вулиці розташування станиці не має перевищувати 60 символів")]
        public string Street { get; set; }

        [Required, MaxLength(10, ErrorMessage = "Номер будинку розташування станиці не має перевищувати 10 символів")]
        public string HouseNumber { get; set; }

        [MaxLength(10, ErrorMessage = "Номер офісу/квартири розташування станиці не має перевищувати 10 символів")]
        public string OfficeNumber { get; set; }

        [MaxLength(7, ErrorMessage = "Поштовий індекс станиці не має перевищувати 7 символів")]
        public string PostIndex { get; set; }
        public string Logo { get; set; }
        public bool IsActive { get; set; }
        public int RegionId { get; set; }
        public RegionDTO Region { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanJoin { get; set; }
        public int MemberCount { get; set; }
        public int FollowerCount { get; set; }
        public int AdministrationCount { get; set; }
        public int DocumentsCount { get; set; }
        public IEnumerable<CityDocumentsDTO> CityDocuments { get; set; }
        public IEnumerable<CityMembersDTO> CityMembers { get; set; }
        [JsonIgnore]
        public IEnumerable<CityAdministrationDTO> CityAdministration { get; set; }
        public IEnumerable<CityLegalStatusDTO> CityLegalStatuses { get; set; }
        public IEnumerable<UserRenewalDTO> UserRenewals { get; set; }
    }
}
