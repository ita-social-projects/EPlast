using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class ClubAnnualReport
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
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

        [MaxLength(500, ErrorMessage = "Максимально допустима кількість символів 500")]
        public string KbUSPWishes { get; set; }
        [StringLength(18, ErrorMessage = "Контактний номер куреня повинен містити 12 цифр")]
        public string PhoneNumber { get; set; }
        [MaxLength(50, ErrorMessage = "Email куреня не має перевищувати 50 символів")]
        public string Email { get; set; }
        [MaxLength(256, ErrorMessage = "Посилання на web-сторінку куреня не має перевищувати 256 символів")]
        public string ClubURL { get; set; }

        [MaxLength(60, ErrorMessage = "Назва вулиці розташування куреня не має перевищувати 60 символів")]
        public string Street { get; set; }

        [Required]
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        public Club Club { get; set; }

        public string CreatorId { get; set; }
        public string CreatorFirstName { get; set; }
        public string CreatorLastName { get; set; }
        public string CreatorFatherName { get; set; }
        public User Creator { get; set; }
      
        public ICollection<ClubReportAdmins> ClubReportAdmins { get; set; }
        public ICollection<ClubReportMember> ClubReportMembers { get; set; }
    }
}
