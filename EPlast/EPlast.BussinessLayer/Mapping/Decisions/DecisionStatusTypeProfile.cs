using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping
{
    public class DecisionStatusTypeProfile : Profile
    {
        public DecisionStatusTypeProfile()
        {
            CreateMap<Organization, OrganizationDTO>().ReverseMap();
        }
    }
}