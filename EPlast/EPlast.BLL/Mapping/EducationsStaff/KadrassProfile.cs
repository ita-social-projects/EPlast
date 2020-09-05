using AutoMapper;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.DataAccess.Entities.EducatorsStaff;
using EPlast.DataAccess.Repositories.Realizations.Base;

namespace EPlast.BLL.Mapping.EducationsStaff
{
    public class KadrassProfile:Profile
    {
        public KadrassProfile()
        {
            CreateMap<KadraVykhovnykiv, KadraVykhovnykivDTO>().ReverseMap();

        }
        
    }
}
