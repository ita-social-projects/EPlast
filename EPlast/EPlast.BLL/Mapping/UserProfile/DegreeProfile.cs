using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.UserProfile
{
    public class DegreeProfile : Profile
    {
        public DegreeProfile()
        {
            CreateMap<Degree, DegreeDto>().ReverseMap();
        }
    }
}
