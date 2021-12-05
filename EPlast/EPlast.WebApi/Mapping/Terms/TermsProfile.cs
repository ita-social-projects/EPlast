using AutoMapper;
using EPlast.BLL.DTO.Terms;
using EPlast.WebApi.Models.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Mapping.Terms
{
    public class TermsProfile:Profile
    {
        public TermsProfile()
        {
            CreateMap<TermsDTO, TermsViewModel>().ReverseMap();
        }
    }
}