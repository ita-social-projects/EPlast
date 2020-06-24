using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.BusinessLogicLayer.Mapping.UserProfile
{
    public class ReligionProfile : Profile
    {
        public ReligionProfile()
        {
            CreateMap<Religion, ReligionDTO>().ReverseMap();
        }
    }
}
