using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping.UserProfile
{
    public class DegreeProfile : Profile
    {
        public DegreeProfile()
        {
            CreateMap<Degree, DegreeDTO>().ReverseMap();
        }
    }
}
