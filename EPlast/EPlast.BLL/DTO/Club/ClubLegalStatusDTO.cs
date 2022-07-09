using System;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.DTO.Club
{
    public class ClubLegalStatusDto
    {
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateFinish { get; set; }
        public ClubLegalStatusType LegalStatusType { get; set; }
        public int CityId { get; set; }
    }
}