using EPlast.WebApi.Models.Region;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.Models.City
{
    public class CityViewModel
    {
        public int ID { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Phone, StringLength(18)]
        public string PhoneNumber { get; set; }

        [Required, EmailAddress, MaxLength(50)]
        public string Email { get; set; }

        [Url, MaxLength(256)]
        public string CityURL { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required, MaxLength(50)]
        public string Address { get; set; }
        public string Logo { get; set; }
        [Required]
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
