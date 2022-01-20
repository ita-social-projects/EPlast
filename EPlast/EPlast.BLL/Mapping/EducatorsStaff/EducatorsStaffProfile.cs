using AutoMapper;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.DataAccess.Entities.EducatorsStaff;

namespace EPlast.BLL.Mapping.EducationsStaff
{
    public class EducatorsStaffProfile:Profile
    {
        public EducatorsStaffProfile()
        {
            CreateMap<EducatorsStaff, EducatorsStaffDTO>().ReverseMap();
            CreateMap<EducatorsStaff, EducatorsStaffTableObject >().ForMember(dest => dest.Id, act => act.MapFrom(src => src.ID))
                .ForMember(dest => dest.UserName, act => act.MapFrom(src => src.User.FirstName + " " + src.User.LastName));
        }
        
    }
}
