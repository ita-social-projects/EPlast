﻿using EPlast.BLL.DTO.AnnualReport;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.GoverningBody.Announcement
{
    public class GoverningBodyAnnouncementUserDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public UserDTO User { get; set; }
        public IEnumerable<GoverningBodyAnnouncementImageDTO> Images { get; set; }
        public bool ImagesPresent { get; set; }
    }
}
