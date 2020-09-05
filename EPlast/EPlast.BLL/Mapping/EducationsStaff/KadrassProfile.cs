using AutoMapper;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.DataAccess.Entities.EducatorsStaff;

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
