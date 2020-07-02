using System;

namespace EPlast.BLL.DTO.City
{
    public class CityMembersDTO
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public CityUserDTO User { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
