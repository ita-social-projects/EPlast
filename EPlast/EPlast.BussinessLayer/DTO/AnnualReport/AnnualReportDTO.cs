using EPlast.BussinessLayer.DTO.City;
using EPlast.BussinessLayer.DTO.UserProfiles;
using System;

namespace EPlast.BussinessLayer.DTO
{
    public class AnnualReportDTO
    {
        public int ID { get; set; }

        public DateTime Date { get; set; }

        public AnnualReportStatusDTO Status { get; set; }

        public int NumberOfSeatsInCity { get; set; }

        public int NumberOfSeatsPtashat { get; set; }

        public int NumberOfIndependentRiy { get; set; }

        public int NumberOfClubs { get; set; }

        public int NumberOfIndependentGroups { get; set; }

        public int NumberOfTeachers { get; set; }

        public int NumberOfAdministrators { get; set; }

        public int NumberOfTeacherAdministrators { get; set; }

        public int NumberOfBeneficiaries { get; set; }

        public int NumberOfPlastpryiatMembers { get; set; }

        public int NumberOfHonoraryMembers { get; set; }

        public int PublicFunds { get; set; }

        public int ContributionFunds { get; set; }

        public int PlastSalary { get; set; }

        public int SponsorshipFunds { get; set; }

        public string ListProperty { get; set; }

        public string ImprovementNeeds { get; set; }

        public MembersStatisticDTO MembersStatistic { get; set; }

        public CityManagementDTO CityManagement { get; set; }

        public string UserId { get; set; }
        public UserDTO User { get; set; }

        public int CityId { get; set; }
        public CityDTO City { get; set; }
    }
}
