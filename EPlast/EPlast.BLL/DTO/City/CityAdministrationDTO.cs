using EPlast.BLL.DTO.AnnualReport;
using EPlast.DataAccess.Entities;
using System;

namespace EPlast.BLL.DTO.City
{
    public class CityAdministrationDTO
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CityId { get; set; }
        public CityDTO City { get; set; }
        public string UserId { get; set; }
        public UserDTO User { get; set; }
        public int AdminTypeId { get; set; }
        public AdminType AdminType { get; set; }
        public CityManagement CityManagement { get; set; }
    }
}
