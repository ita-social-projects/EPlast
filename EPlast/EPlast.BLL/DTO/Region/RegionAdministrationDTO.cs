using System;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.BLL.DTO.Region
{
    public class RegionAdministrationDTO
    {
        public int ID { get; set; }
        public AdminTypeDTO AdminType { get; set; }
        public UserDTO User { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        //public RegionDTO Region { get; set; }
    }
}