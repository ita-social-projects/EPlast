using EPlast.BLL.DTO.City;
using System;
using EPlast.BLL.DTO.PrecautionsDTO;
using EPlast.Resources;

namespace EPlast.BLL
{
    public class UserPrecautionDTO
    {
        public int Id { get; set; }
        public int PrecautionId { get; set; }
        public PrecautionDTO Precaution { get; set; }
        public string Reporter { get; set; }
        public string Reason { get; set; }
        public UserPrecautionStatus Status { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public PrecautionUserDTO User { get; set; }
        public DateTime EndDate => Date.AddMonths(Precaution.MonthsPeriod);
        public bool IsActive => EndDate > DateTime.Now;
    }
}
