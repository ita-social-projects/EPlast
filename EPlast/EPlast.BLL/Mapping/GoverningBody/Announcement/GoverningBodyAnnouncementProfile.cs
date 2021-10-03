using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.DataAccess.Entities.GoverningBody.Announcement;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Mapping.GoverningBody.Announcement
{
    public class GoverningBodyAnnouncementProfile : Profile
    {
        public GoverningBodyAnnouncementProfile()
        {
            CreateMap<GoverningBodyAnnouncement, GoverningBodyAnnouncementDTO>().ReverseMap();
        }
    }
}
