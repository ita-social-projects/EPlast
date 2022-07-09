using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.UserProfile
{
    public class NationalityProfile : Profile
    {
        public NationalityProfile()
        {
            CreateMap<Nationality, NationalityDto>().ReverseMap();
        }
    }
}
