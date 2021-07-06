using EPlast.WebApi.Models.Admin;
using System;

namespace EPlast.WebApi.Models.GoverningBody
{
    public class GoverningBodyAdministrationViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public GoverningBodyUserViewModel User { get; set; }
        public int CityId { get; set; }
        public int AdminTypeId { get; set; }
        public AdminTypeViewModel AdminType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
