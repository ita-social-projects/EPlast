using System;
using EPlast.BusinessLogicLayer.DTO.Admin;
using EPlast.BusinessLogicLayer.DTO.UserProfiles;

namespace EPlast.BusinessLogicLayer.DTO
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