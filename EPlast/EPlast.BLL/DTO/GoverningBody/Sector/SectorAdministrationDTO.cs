using System;
using EPlast.BLL.DTO.Admin;

namespace EPlast.BLL.DTO.GoverningBody.Sector
{
    public class SectorAdministrationDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public SectorUserDto User { get; set; }
        public int SectorId { get; set; }
        public SectorDto Sector { get; set; }
        public int AdminTypeId { get; set; }
        public AdminTypeDto AdminType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Status { get; set; }
        public string WorkEmail { get; set; }
    }
}
