using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping
{
    public class DecisionTargetProfile : Profile
    {
        public DecisionTargetProfile()
        {
            CreateMap<DecesionTarget, DecisionTargetDto>().ReverseMap();
        }
    }
}