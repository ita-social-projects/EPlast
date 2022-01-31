using System;
using System.Collections.Generic;
using AutoMapper;
using EPlast.BLL.DTO.Terms;

namespace EPlast.BLL.Mapping.Terms
{
    public class TermsProfile:Profile
    {
        public TermsProfile()
        {
            CreateMap<DataAccess.Entities.Terms, TermsDTO>().ReverseMap();
        }
    }
}   