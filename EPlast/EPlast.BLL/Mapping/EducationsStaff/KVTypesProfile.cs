using AutoMapper;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.DataAccess.Entities.EducatorsStaff;

namespace EPlast.BLL.Mapping.EducationsStaff
{
    public class KVTypesProfile:Profile
    {
        public KVTypesProfile()
        {
            CreateMap<KVTypes, KVTypeDTO>( )
                .ForMember(d => d.KVTypeName, o => o.MapFrom(s => s.KVTypeName))
                .ForMember(d => d.ID, o => o.MapFrom(s => s.ID))
                .ReverseMap();
        }

       
    }
}
