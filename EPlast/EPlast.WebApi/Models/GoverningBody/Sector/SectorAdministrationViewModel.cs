using EPlast.WebApi.Models.Admin;
using System;

namespace EPlast.WebApi.Models.GoverningBody.Sector
{
    public class SectorAdministrationViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public SectorUserViewModel User { get; set; }
        public int SectorId { get; set; }
        public int AdminTypeId { get; set; }
        public AdminTypeViewModel AdminType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string WorkEmail { get; set; }
    }
}