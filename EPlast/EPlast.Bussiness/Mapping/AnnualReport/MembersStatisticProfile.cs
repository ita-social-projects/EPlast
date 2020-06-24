using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.AnnualReport;
using EPlast.DataAccess.Entities;

namespace EPlast.BusinessLogicLayer.Mapping.AnnualReport
{
    public class MembersStatisticProfile : Profile
    {
        public MembersStatisticProfile()
        {
            CreateMap<MembersStatistic, MembersStatisticDTO>().ReverseMap();
        }
    }
}