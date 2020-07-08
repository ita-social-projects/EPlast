using EPlast.DataAccess.Entities;
using System;

namespace EPlast.BLL.DTO.City
{
    public class CityLegalStatusDTO
    {
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateFinish { get; set; }
        public CityLegalStatusType LegalStatusType { get; set; }
        public int CityId { get; set; }
    }
}
