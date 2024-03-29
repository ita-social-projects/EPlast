﻿using AutoMapper;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.GoverningBody;

namespace EPlast.BLL.Mapping.GoverningBody
{
    public class GoverningBodyDocumentsProfile : Profile
    {
        public GoverningBodyDocumentsProfile()
        {
            CreateMap<GoverningBodyDocuments, GoverningBodyDocumentsDto>().ReverseMap();
            CreateMap<GoverningBodyDocumentType, GoverningBodyDocumentTypeDto>().ReverseMap();
        }
    }
}
