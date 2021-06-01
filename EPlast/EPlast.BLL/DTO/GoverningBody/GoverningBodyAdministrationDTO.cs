using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.UserProfiles;
using System;

namespace EPlast.BLL.DTO.GoverningBody
{
    public class GoverningBodyAdministrationDTO
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public GoverningBodyUserDTO User { get; set; }
        public int GoverningBodyId { get; set; }
        public GoverningBodyDTO GoverningBody { get; set; }
        public int AdminTypeId { get; set; }
        public AdminTypeDTO AdminType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Status { get; set; }
    }
}
