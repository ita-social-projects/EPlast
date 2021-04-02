using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class MembersStatistic
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfPtashata { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfNovatstva { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfUnatstvaNoname { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfUnatstvaSupporters { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfUnatstvaMembers { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfUnatstvaProspectors { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfUnatstvaSkobVirlyts { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfSeniorPlastynSupporters { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfSeniorPlastynMembers { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfSeigneurSupporters { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfSeigneurMembers { get; set; }

        public int AnnualReportId { get; set; }
        public AnnualReport AnnualReport { get; set; }
    }
}
