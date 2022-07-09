using AutoMapper;
using EPlast.BLL.DTO.Club;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Club
{
    class ClubReportUserProfile : Profile
    {
        public ClubReportUserProfile()
        {
            CreateMap<DatabaseEntities.User, ClubReportUserDto>().ReverseMap();
        }
    }
}

