using EPlast.Models.Enums;
using EPlast.ViewModels.City;
using EPlast.ViewModels.UserInformation.UserProfile;
using System.ComponentModel.DataAnnotations;

namespace EPlast.ViewModels
{
    public class CityManagementViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Оберіть правовий статус осередку")]
        public CityLegalStatusType CityLegalStatusNew { get; set; }

        public int? CityLegalStatusOldId { get; set; }
        public CityLegalStatusViewModel CityLegalStatusOld { get; set; }

        public string UserId { get; set; }
        public UserViewModel CityAdminNew { get; set; }

        public int? CityAdminOldId { get; set; }
        public CityAdministrationViewModel CityAdminOld { get; set; }

        public int AnnualReportId { get; set; }
        public AnnualReportViewModel AnnualReport { get; set; }
    }
}