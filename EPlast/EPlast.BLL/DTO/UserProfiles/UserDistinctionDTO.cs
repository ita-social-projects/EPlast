using EPlast.BLL.DTO.City;
using System;

namespace EPlast.BLL
{
    public class UserDistinctionDTO
    {
        public int Id { get; set; }
        public int DistinctionId { get; set; }
        public DistinctionDTO Distinction { get; set; }
        public string Reporter { get; set; }
        public string Reason { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public CityUserDTO User { get; set; }
    }
}
