using EPlast.DataAccess.Entities;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.DTO
{
    public class CityDTO
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
        public Region Region { get; set; }
        public string Logo { get; set; }
        public ICollection<CityDocuments> CityDocuments { get; set; }
        public ICollection<CityMembers> CityMembers { get; set; }
        public ICollection<UnconfirmedCityMember> UnconfirmedCityMember { get; set; }
        public ICollection<CityAdministration> CityAdministration { get; set; }
        public ICollection<AnnualReport> AnnualReports { get; set; }
        public ICollection<CityLegalStatus> CityLegalStatuses { get; set; }
    }
}
