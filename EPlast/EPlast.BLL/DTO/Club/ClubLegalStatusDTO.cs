using EPlast.DataAccess.Entities;
using System;

namespace EPlast.BLL.DTO.Club
{
    public class ClubLegalStatusDTO
    {
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateFinish { get; set; }
        public ClubLegalStatusType LegalStatusType { get; set; }
        public int CityId { get; set; }
    }
}