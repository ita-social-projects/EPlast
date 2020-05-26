using EPlast.DataAccess.Entities;
using System;

namespace EPlast.BussinessLayer.DTO
{
    public class CityAdministrationDTO
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public string UserId { get; set; }
        public UserDTO User { get; set; }
        public int AdminTypeId { get; set; }
        public AdminType AdminType { get; set; }
        public CityManagement CityManagement { get; set; }
    }
}
