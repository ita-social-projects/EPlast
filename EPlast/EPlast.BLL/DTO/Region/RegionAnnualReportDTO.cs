using System;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.DTO.Region
{
    public class RegionAnnualReportDto
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfSeatsPtashat { get; set; }
        public int NumberOfPtashata { get; set; }
        public int NumberOfNovatstva { get; set; }
        public int NumberOfUnatstvaNoname { get; set; }
        public int NumberOfUnatstvaSupporters { get; set; }
        public int NumberOfUnatstvaMembers { get; set; }
        public int NumberOfUnatstvaProspectors { get; set; }
        public int NumberOfUnatstvaSkobVirlyts { get; set; }
        public int NumberOfSeniorPlastynSupporters { get; set; }
        public int NumberOfSeniorPlastynMembers { get; set; }
        public int NumberOfSeigneurSupporters { get; set; }
        public int NumberOfSeigneurMembers { get; set; }
        public int NumberOfIndependentRiy { get; set; }
        public int NumberOfClubs { get; set; }
        public int NumberOfIndependentGroups { get; set; }
        public int NumberOfTeachers { get; set; }
        public int NumberOfAdministrators { get; set; }
        public int NumberOfTeacherAdministrators { get; set; }
        public int NumberOfBeneficiaries { get; set; }
        public int NumberOfPlastpryiatMembers { get; set; }
        public int NumberOfHonoraryMembers { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public string CreatorId { get; set; }
        public string CreatorFirstName { get; set; }
        public string CreatorLastName { get; set; }
        public string CreatorFatherName { get; set; }
        public string StateOfPreparation { get; set; }
        public string Characteristic { get; set; }
        public string StatusOfStrategy { get; set; }
        public string InvolvementOfVolunteers { get; set; }
        public string TrainedNeeds { get; set; }
        public string PublicFunding { get; set; }
        public string ChurchCooperation { get; set; }
        public string Fundraising { get; set; }
        public string SocialProjects { get; set; }
        public string ProblemSituations { get; set; }
        public string ImportantNeeds { get; set; }
        public string SuccessStories { get; set; }
        public AnnualReportStatus Status { get; set; }

    }
}
