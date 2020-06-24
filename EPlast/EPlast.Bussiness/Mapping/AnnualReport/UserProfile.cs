using AutoMapper;
using EPlast.Bussiness.DTO.AnnualReport;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.Bussiness.Mapping.AnnualReport
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<DatabaseEntities.User, UserDTO>().ReverseMap();
        }
    }
}