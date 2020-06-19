using System.ComponentModel.DataAnnotations;

namespace EPlast.BussinessLayer.DTO.AnnualReport
{
    public class CityManagementDTO
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Оберіть правовий статус осередку")]
        public CityLegalStatusTypeDTO CityLegalStatusNew { get; set; }

        public int? CityLegalStatusOldId { get; set; }

        public string UserId { get; set; }
        public UserDTO CityAdminNew { get; set; }

        public int? CityAdminOldId { get; set; }

        public int AnnualReportId { get; set; }
    }
}