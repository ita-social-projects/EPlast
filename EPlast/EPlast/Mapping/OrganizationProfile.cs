using System.Collections.Generic;
using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.Models;

namespace EPlast.Mapping
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<Organization, OrganizationDTO>();
            CreateMap<OrganizationDTO, Organization>();
        }
    }
}