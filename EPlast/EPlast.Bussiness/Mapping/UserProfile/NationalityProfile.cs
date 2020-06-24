using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.BusinessLogicLayer.Mapping.UserProfile
{
    public class NationalityProfile : Profile
    {
        public NationalityProfile()
        {
            CreateMap<Nationality, NationalityDTO>().ReverseMap();
        }
    }
}
