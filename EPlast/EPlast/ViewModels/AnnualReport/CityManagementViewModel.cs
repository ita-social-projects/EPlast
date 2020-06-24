using System.ComponentModel.DataAnnotations;

namespace EPlast.ViewModels.AnnualReport
{
    public class CityManagementViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Оберіть правовий статус осередку")]
        public CityLegalStatusTypeViewModel CityLegalStatusNew { get; set; }

        public int? CityLegalStatusOldId { get; set; }

        public string UserId { get; set; }
        public UserViewModel CityAdminNew { get; set; }

        public int? CityAdminOldId { get; set; }

        public int AnnualReportId { get; set; }
        public AnnualReportViewModel AnnualReport { get; set; }
    }
}