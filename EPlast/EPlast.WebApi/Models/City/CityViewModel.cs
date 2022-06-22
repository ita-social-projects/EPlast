using EPlast.WebApi.Models.Region;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.City
{
    public class CityViewModel
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
        public string Address { get; set; }
        public string Logo { get; set; }
        public string Region { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanJoin { get; set; }
        public bool IsActive { get; set; }
        public CityAdministrationViewModel Head { get; set; }
        public CityAdministrationViewModel HeadDeputy { get; set; }
        public IEnumerable<CityAdministrationViewModel> Administration { get; set; }
        public IEnumerable<CityMembersViewModel> Members { get; set; }
        public IEnumerable<CityMembersViewModel> Followers { get; set; }
        public IEnumerable<CityDocumentsViewModel> Documents { get; set; }

        public int MemberCount { get; set; }
        public int FollowerCount { get; set; }
        public int AdministrationCount { get; set; }
        public int DocumentsCount { get; set; }
        public int Level { get; set; }
    }
}
