using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping.UserProfile
{
    public class ReligionProfile : Profile
    {
        public ReligionProfile()
        {
            CreateMap<Religion, ReligionDTO>().ReverseMap();
        }
    }
}
