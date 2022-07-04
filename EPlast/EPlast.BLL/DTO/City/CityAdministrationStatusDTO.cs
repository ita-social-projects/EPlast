using System;
using EPlast.BLL.DTO.Admin;

namespace EPlast.BLL.DTO.City
{
    public class CityAdministrationStatusDto
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public CityUserDto User { get; set; }
        public int CityId { get; set; }
        public CityDto City { get; set; }
        public int AdminTypeId { get; set; }
        public AdminTypeDto AdminType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Status { get; set; }
    }
}
