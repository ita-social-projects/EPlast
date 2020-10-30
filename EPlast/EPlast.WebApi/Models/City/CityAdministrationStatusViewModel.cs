using EPlast.WebApi.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.WebApi.Models.City
{
    public class CityAdministrationStatusViewModel
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public CityUserViewModel User { get; set; }
        public int CityId { get; set; }
        public CityViewModel City { get; set; }
        public int AdminTypeId { get; set; }
        public AdminTypeViewModel AdminType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
