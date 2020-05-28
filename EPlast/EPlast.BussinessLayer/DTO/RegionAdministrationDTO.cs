using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BussinessLayer.DTO
{
    public class RegionAdministrationDTO
    {
        public int ID { get; set; }
        public AdminTypeDTO AdminType { get; set; }
        public UserDTO User { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public RegionDTO Region { get; set; }
    }
}
