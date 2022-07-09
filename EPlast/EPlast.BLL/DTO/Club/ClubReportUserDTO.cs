using System.Collections.Generic;
using EPlast.BLL.DTO.ActiveMembership;
using EPlast.DataAccess.Entities;


namespace EPlast.BLL.DTO.Club
{
    public class ClubReportUserDto
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string ImagePath { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public UserPlastDegreeDto UserPlastDegrees { get; set; }
        public ICollection<ClubReportCityMembersDto> CityMembers { get; set; }
        public ClubReportPlastDegreesDto ClubReportPlastDegrees { get; set; }
        public ClubReportCitiesDto ClubReportCities { get; set; }
    }
}
