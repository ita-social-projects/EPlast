using AutoMapper;
using EPlast.Bussiness.DTO;
using EPlast.DataAccess.Entities;

namespace EPlast.Bussiness.Mapping
{
    public class DecisionStatusTypeProfile : Profile
    {
        public DecisionStatusTypeProfile()
        {
            CreateMap<Organization, OrganizationDTO>().ReverseMap();
        }
    }
}