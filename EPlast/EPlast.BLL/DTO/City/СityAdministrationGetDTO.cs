using EPlast.BLL.DTO.Admin;
using System;

namespace EPlast.BLL.DTO.City
{
    public class CityAdministrationGetDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public CityUserDTO User { get; set; }
        public int CityId { get; set; }
        public int AdminTypeId { get; set; }
        public AdminTypeDTO AdminType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Status { get; set; }
    }
}
