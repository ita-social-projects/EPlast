using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Terms;
using EPlast.WebApi.Models.Terms;

namespace EPlast.WebApi.Mapping.Terms
{
    public class TermsProfile:Profile
    {
        public TermsProfile()
        {
            CreateMap<TermsDto, TermsViewModel>().ReverseMap();
        }
    }
}