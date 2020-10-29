using EPlast.BLL.DTO.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.DTO.City
{
    public class CityAdministrationStatusDTO
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public CityUserDTO User { get; set; }
        public int CityId { get; set; }
        public CityDTO City { get; set; }
        public int AdminTypeId { get; set; }
        public AdminTypeDTO AdminType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Status { get; set; }
    }
}
