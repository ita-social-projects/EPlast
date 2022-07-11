using System;
using System.Collections.Generic;
using System.Text;
using EPlast.BLL.DTO.Admin;

namespace EPlast.BLL.DTO.Club
{
    public class ClubReportAdministrationDto
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public ClubReportUserDto User { get; set; }
        public int ClubId { get; set; }
        public int AdminTypeId { get; set; }
        public AdminTypeDto AdminType { get; set; }
        public bool Status { get; set; }
    }
}
