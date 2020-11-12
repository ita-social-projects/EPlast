using System;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.City;

namespace EPlast.BLL.DTO.Region
{
    public class RegionAdministrationDTO
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public CityUserDTO User { get; set; }
        public int CityId { get; set; }
        public int AdminTypeId { get; set; }
        public AdminTypeDTO AdminType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int RegionId { get; set; }
        public RegionDTO Region { get; set; }
        public bool Status { get; set; }
    }
}