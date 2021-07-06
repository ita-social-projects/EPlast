using EPlast.WebApi.Models.Region;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EPlast.WebApi.Models.City
{
    public class CityViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CityURL { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string OfficeNumber { get; set; }
        public string PostIndex { get; set; }
        public string Logo { get; set; }
        public string Region { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanJoin { get; set; }
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
    }
}
