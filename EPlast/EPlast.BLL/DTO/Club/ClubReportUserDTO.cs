using EPlast.BLL.DTO.ActiveMembership;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;


namespace EPlast.BLL.DTO.Club
{
    public class ClubReportUserDTO
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string ImagePath { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<UserPlastDegreeDTO> UserPlastDegrees { get; set; }
        public ICollection<ClubReportCitiMembersDTO> CityMembers { get; set; }
        public ClubReportPlastDegreesDTO ClubReportPlastDegrees { get; set; }
        public ClubReportCitiesDTO ClubReportCities { get; set; }
    }
}
