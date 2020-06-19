using AutoMapper;
using EPlast.BussinessLayer.DTO.AnnualReport;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping.AnnualReport
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<DatabaseEntities.User, UserAnnualReportDTO>().ReverseMap();
        }
    }
}