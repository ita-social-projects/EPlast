using AutoMapper;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.DataAccess.Entities.EducatorsStaff;

namespace EPlast.BLL.Mapping.EducationsStaff
{
    public class KadrasTypesProfile:Profile
    {
        public KadrasTypesProfile()
        {
            CreateMap<KadraVykhovnykivTypes, KadraVykhovnykivTypes>( )
                .ReverseMap();
        }

       
    }
}
