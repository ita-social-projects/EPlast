using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.AnnualReport;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels.AnnualReport;

namespace EPlast.Mapping.AnnualReport
{
    public class MembersStatisticProfile : Profile
    {
        public MembersStatisticProfile()
        {
            CreateMap<MembersStatistic, MembersStatisticDTO>().ReverseMap();
            CreateMap<MembersStatisticViewModel, MembersStatisticDTO>().ReverseMap();
        }
    }
}