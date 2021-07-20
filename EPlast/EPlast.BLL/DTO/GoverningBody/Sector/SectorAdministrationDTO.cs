using System;
using EPlast.BLL.DTO.Admin;

namespace EPlast.BLL.DTO.GoverningBody.Sector
{
    public class SectorAdministrationDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public SectorUserDTO User { get; set; }
        public int SectorId { get; set; }
        public SectorDTO Sector { get; set; }
        public int AdminTypeId { get; set; }
        public AdminTypeDTO AdminType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Status { get; set; }
    }
}
