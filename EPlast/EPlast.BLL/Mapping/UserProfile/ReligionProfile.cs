using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.UserProfile
{
    public class ReligionProfile : Profile
    {
        public ReligionProfile()
        {
            CreateMap<Religion, ReligionDto>().ReverseMap();
        }
    }
}
