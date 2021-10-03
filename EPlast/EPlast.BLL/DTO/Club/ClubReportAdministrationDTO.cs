using EPlast.BLL.DTO.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.Club
{
    public class ClubReportAdministrationDTO
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public ClubReportUserDTO User { get; set; }
        public int ClubId { get; set; }
        public int AdminTypeId { get; set; }
        public AdminTypeDTO AdminType { get; set; }
        public bool Status { get; set; }
    }
}
