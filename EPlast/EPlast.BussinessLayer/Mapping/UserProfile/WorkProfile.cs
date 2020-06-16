using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping.UserProfile
{
    public class WorkProfile : Profile
    {
        public WorkProfile()
        {
            CreateMap<Work, WorkDTO>().ReverseMap();
        }
    }
}
