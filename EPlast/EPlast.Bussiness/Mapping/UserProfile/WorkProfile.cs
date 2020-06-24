using AutoMapper;
using EPlast.Bussiness.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.Bussiness.Mapping.UserProfile
{
    public class WorkProfile : Profile
    {
        public WorkProfile()
        {
            CreateMap<Work, WorkDTO>().ReverseMap();
        }
    }
}
