using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping
{
    public class DecisionTargetProfile : Profile
    {
        public DecisionTargetProfile()
        {
            CreateMap<DecesionTarget, DecisionTargetDTO>();
            CreateMap<DecisionTargetDTO, DecesionTarget>();
        }
    }
}