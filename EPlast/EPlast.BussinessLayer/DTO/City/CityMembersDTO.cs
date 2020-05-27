using System;

namespace EPlast.BussinessLayer.DTO.City
{
    public class CityMembersDTO
    {
        public int ID { get; set; }
        public int CityId { get; set; }
        public CityDTO City { get; set; }
        public string UserId { get; set; }
        public UserDTO User { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
