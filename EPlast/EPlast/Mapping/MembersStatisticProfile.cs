using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;

namespace EPlast.Mapping
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