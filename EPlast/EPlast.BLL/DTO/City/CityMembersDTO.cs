using System;

namespace EPlast.BLL.DTO.City
{
    public class CityMembersDto
    {
        public int ID { get; set; }
        public bool IsApproved { get; set; }
        public bool WasInRegisteredUserRole { get; set; }
        public string UserId { get; set; }
        public CityUserDto User { get; set; }
        public string CityId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
