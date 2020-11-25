using EPlast.DataAccess.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.Club
{
    public class ClubAnnualReportDTO
    {
        public int ID { get; set; }
        public AnnualReportStatus Status { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, int.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int CurrentClubMembers { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, int.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int CurrentClubFollowers { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, int.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int ClubEnteredMembersCount { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, int.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int ClubLeftMembersCount { get; set; }

        [MaxLength(200, ErrorMessage = "Максимально допустима кількість символів 200")]
        public string ClubCenters { get; set; }

        [MaxLength(200, ErrorMessage = "Максимально допустима кількість символів 200")]
        public string ClubContacts { get; set; }

        [MaxLength(200, ErrorMessage = "Максимально допустима кількість символів 200")]
        public string ClubPage { get; set; }

        [MaxLength(500, ErrorMessage = "Максимально допустима кількість символів 500")]
        public string KbUSPWishes { get; set; }

        public int ClubId { get; set; }
        public ClubDTO Club { get; set; }
        public string ClubMembersSummary { get; set; }
        public string ClubAdminContacts { get; set; }
        public DateTime Date { get; set; }
    }
}
