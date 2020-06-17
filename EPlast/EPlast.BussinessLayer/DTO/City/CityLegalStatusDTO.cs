using EPlast.BussinessLayer.DTO.City;
using System;

namespace EPlast.BussinessLayer.DTO
{
    public class CityLegalStatusDTO
    {
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateFinish { get; set; }
        public CityLegalStatusTypeDTO LegalStatusType { get; set; }

        public int CityId { get; set; }
        public CityDTO City { get; set; }

        public CityManagementDTO CityManagement { get; set; }
    }
}