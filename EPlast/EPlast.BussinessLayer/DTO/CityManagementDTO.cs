using EPlast.BussinessLayer.DTO.City;

namespace EPlast.BussinessLayer.DTO
{
    public class CityManagementDTO
    {
        public int ID { get; set; }

        public CityLegalStatusType CityLegalStatusNew { get; set; }

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