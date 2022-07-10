using AutoMapper;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.DataAccess.Entities.EducatorsStaff;

namespace EPlast.BLL.Mapping.EducationsStaff
{
    public class EducatorsStaffTypesProfile:Profile
    {
        public EducatorsStaffTypesProfile()
        {
            CreateMap<EducatorsStaffTypes, EducatorsStaffTypesDto>()
                .ReverseMap();
        }

       
    }
}
