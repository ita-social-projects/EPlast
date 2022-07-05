using AutoMapper;
using EPlast.BLL.DTO.AnnualReport;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.AnnualReport
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<DatabaseEntities.User, UserDto>().ReverseMap();
        }
    }
}