using AutoMapper;
using EPlast.BussinessLayer.DTO.AnnualReport;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping.AnnualReport
{
    public class MembersStatisticProfile : Profile
    {
        public MembersStatisticProfile()
        {
            CreateMap<MembersStatistic, MembersStatisticDTO>().ReverseMap();
        }
    }
}