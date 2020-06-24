using AutoMapper;
using EPlast.Bussiness.DTO;
using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Mapping
{
    public class DecisionProfile : Profile
    {
        public DecisionProfile()
        {
            CreateMap<Decesion, DecisionDTO>()
                .ForMember(d => d.DecisionStatusType, o => o.MapFrom(s => s.DecesionStatusType))
                .ForMember(d => d.DecisionTarget, o => o.MapFrom(s => s.DecesionTarget))
                .ForMember(d => d.Organization, o => o.MapFrom(s => s.Organization))
                .ReverseMap();
        }
    }
}