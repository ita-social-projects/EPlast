using System;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.PrecautionsDTO;

namespace EPlast.BLL
{
    public class UserPrecautionDto
    {
        public int Id { get; set; }
        public int PrecautionId { get; set; }
        public PrecautionDto Precaution { get; set; }
        public string Reporter { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public PrecautionUserDto User { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
