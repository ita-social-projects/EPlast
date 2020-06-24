using AutoMapper;
using EPlast.Bussiness.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.Bussiness.Mapping.UserProfile
{
    public class DegreeProfile : Profile
    {
        public DegreeProfile()
        {
            CreateMap<Degree, DegreeDTO>().ReverseMap();
        }
    }
}
