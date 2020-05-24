using System.Collections.Generic;
using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<Organization, OrganizationDTO>();
            CreateMap<List<OrganizationDTO>, List<Organization>>();
        }
    }
}