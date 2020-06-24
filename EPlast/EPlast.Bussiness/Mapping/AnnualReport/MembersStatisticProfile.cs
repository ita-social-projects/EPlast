using AutoMapper;
using EPlast.Bussiness.DTO.AnnualReport;
using EPlast.DataAccess.Entities;

namespace EPlast.Bussiness.Mapping.AnnualReport
{
    public class MembersStatisticProfile : Profile
    {
        public MembersStatisticProfile()
        {
            CreateMap<MembersStatistic, MembersStatisticDTO>().ReverseMap();
        }
    }
}