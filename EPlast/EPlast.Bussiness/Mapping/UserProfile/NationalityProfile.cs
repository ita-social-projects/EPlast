using AutoMapper;
using EPlast.Bussiness.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.Bussiness.Mapping.UserProfile
{
    public class NationalityProfile : Profile
    {
        public NationalityProfile()
        {
            CreateMap<Nationality, NationalityDTO>().ReverseMap();
        }
    }
}
