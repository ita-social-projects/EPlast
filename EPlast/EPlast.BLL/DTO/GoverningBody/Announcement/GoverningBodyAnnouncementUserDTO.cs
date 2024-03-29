﻿using System;
using System.Collections.Generic;
using System.Text;
using EPlast.BLL.DTO.AnnualReport;

namespace EPlast.BLL.DTO.GoverningBody.Announcement
{
    public class GoverningBodyAnnouncementUserDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public UserDto User { get; set; }
        public int GoverningBodyId { get; set; }
        public int? SectorId { get; set; }
        public bool ImagesPresent { get; set; }
        public bool IsPined { get; set; }
    }
}
