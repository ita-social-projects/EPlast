using EPlast.WebApi.Models.Admin;
using System;

namespace EPlast.WebApi.Models.City
{
    public class CityAdministrationViewModel
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public CityUserViewModel User { get; set; }
        public int AdminTypeId { get; set; }
        public AdminTypeViewModel AdminType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
