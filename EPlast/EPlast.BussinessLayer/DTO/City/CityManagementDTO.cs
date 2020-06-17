using EPlast.BussinessLayer.DTO.City;
using EPlast.BussinessLayer.DTO.UserProfiles;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BussinessLayer.DTO
{
    public class CityManagementDTO
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Оберіть правовий статус осередку")]
        public CityLegalStatusTypeDTO CityLegalStatusNew { get; set; }

        public int? CityLegalStatusOldId { get; set; }
        public CityLegalStatusDTO CityLegalStatusOld { get; set; }

        public string UserId { get; set; }
        public UserDTO CityAdminNew { get; set; }

        public int? CityAdminOldId { get; set; }
        public CityAdministrationDTO CityAdminOld { get; set; }

        public int AnnualReportId { get; set; }
        public AnnualReportDTO AnnualReport { get; set; }
    }
}