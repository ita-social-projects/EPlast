using System;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.BLL.DTO.GoverningBody
{
    public class GoverningBodyAdministrationDto
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public GoverningBodyUserDto User { get; set; }
        public int GoverningBodyId { get; set; }
        public GoverningBodyDto GoverningBody { get; set; }
        public int? AdminTypeId { get; set; }
        public AdminTypeDto AdminType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Status { get; set; }
        public string WorkEmail { get; set; }
        public string GoverningBodyAdminRole { get; set; }
    }
}
