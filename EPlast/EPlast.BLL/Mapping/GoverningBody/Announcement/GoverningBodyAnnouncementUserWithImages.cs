using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.DataAccess.Entities.GoverningBody;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Mapping.GoverningBody.Announcement
{
    public class GoverningBodyAnnouncementUserWithImages : Profile
    {
        public GoverningBodyAnnouncementUserWithImages()
        {
            CreateMap<GoverningBodyAnnouncementUserWithImagesDTO, GoverningBodyAnnouncement>().ReverseMap();
        }
    }
}
