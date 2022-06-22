using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.Club
{
    public class ClubViewModel
    {
        public int ID { get; set; }
        [Required, MaxLength(200, ErrorMessage = "Назва куреня не може бути довшою за 200 символів.")]
        public string Name { get; set; }
        [MaxLength(18, ErrorMessage = "Контактний телефон куреня не може бути довшим за 12 символів.")]
        public string PhoneNumber { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Електронна адрес куреня не може бути довшою за 50 символів.")]
        public string Email { get; set; }
        [MaxLength(256, ErrorMessage = "Посилання куреня не може бути довшим за 256 символів.")]
        public string ClubURL { get; set; }
        [MaxLength(1000, ErrorMessage = "Опис куреня не може бути довшим за 1000 символів.")]
        public string Description { get; set; }
        public string Slogan { get; set; }
        public string Logo { get; set; }
        public bool isActive { get; set; }
        public string Region { get; set; }
        public bool CanJoin { get; set; }
        public int MemberCount { get; set; }
        public int FollowerCount { get; set; }
        public int AdministrationCount { get; set; }
        public int DocumentsCount { get; set; }
        public ClubAdministrationViewModel Head { get; set; }
        public ClubAdministrationViewModel HeadDeputy { get; set; }
        public IEnumerable<ClubAdministrationViewModel> Administration { get; set; }
        public IEnumerable<ClubMembersViewModel> Members { get; set; }
        public IEnumerable<ClubMembersViewModel> Followers { get; set; }
        public IEnumerable<ClubDocumentsViewModel> Documents { get; set; }
    }
}