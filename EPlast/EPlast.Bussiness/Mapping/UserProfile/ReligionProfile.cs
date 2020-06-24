using AutoMapper;
using EPlast.Bussiness.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.Bussiness.Mapping.UserProfile
{
    public class ReligionProfile : Profile
    {
        public ReligionProfile()
        {
            CreateMap<Religion, ReligionDTO>().ReverseMap();
        }
    }
}
