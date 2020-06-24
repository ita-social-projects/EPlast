using AutoMapper;
using EPlast.Bussiness.DTO;
using EPlast.DataAccess.Entities;

namespace EPlast.Bussiness.Mapping
{
    public class DecisionTargetProfile : Profile
    {
        public DecisionTargetProfile()
        {
            CreateMap<DecesionTarget, DecisionTargetDTO>().ReverseMap();
        }
    }
}