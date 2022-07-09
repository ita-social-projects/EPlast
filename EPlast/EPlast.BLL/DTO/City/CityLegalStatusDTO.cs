using System;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.DTO.City
{
    public class CityLegalStatusDto
    {
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateFinish { get; set; }
        public CityLegalStatusType LegalStatusType { get; set; }
        public int CityId { get; set; }
    }
}
