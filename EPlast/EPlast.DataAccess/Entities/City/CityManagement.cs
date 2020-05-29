using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class CityManagement
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Оберіть правовий статус осередку")]
        public CityLegalStatusType CityLegalStatusNew { get; set; } 

        public int? CityLegalStatusOldId { get; set; }
        public CityLegalStatus CityLegalStatusOld { get; set; }

        public string UserId { get; set; }
        public User CityAdminNew { get; set; }

        public int? CityAdminOldId { get; set; }
        public CityAdministration CityAdminOld { get; set; }

        public int AnnualReportId { get; set; }
        public AnnualReport AnnualReport { get; set; }
    }
}