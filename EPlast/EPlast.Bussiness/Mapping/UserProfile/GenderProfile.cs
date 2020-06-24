using AutoMapper;
using EPlast.Bussiness.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.Bussiness.Mapping.UserProfile
{
    public class GenderProfile : Profile
    {
        public GenderProfile()
        {
            CreateMap<Gender, GenderDTO>().ReverseMap();
        }
    }
}
