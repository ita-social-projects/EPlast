using AutoMapper;
using EPlast.BusinessLogicLayer.DTO;
using EPlast.DataAccess.Entities;

namespace EPlast.BusinessLogicLayer.Mapping
{
    public class DecisionStatusTypeProfile : Profile
    {
        public DecisionStatusTypeProfile()
        {
            CreateMap<Organization, OrganizationDTO>().ReverseMap();
        }
    }
}