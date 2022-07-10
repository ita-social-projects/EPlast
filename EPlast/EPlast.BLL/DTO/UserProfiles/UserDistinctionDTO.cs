using System;
using EPlast.BLL.DTO.City;

namespace EPlast.BLL
{
    public class UserDistinctionDto
    {
        public int Id { get; set; }
        public int DistinctionId { get; set; }
        public DistinctionDto Distinction { get; set; }
        public string Reporter { get; set; }
        public string Reason { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public CityUserDto User { get; set; }
    }
}
